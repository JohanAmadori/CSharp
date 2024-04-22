using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;


namespace wpf_application.Pages
{
    public partial class CrudM : Page
    {
        // Objets nécessaires pour SQL
        const string _dsn = "server=localhost;port=3306;database=raptest;username=root;password=;";
        private MySqlConnection _connexion = new MySqlConnection(_dsn);
        private MySqlCommand _command;
        private MySqlDataAdapter _adapter;

        private DataTable _dt;  // Ne fait pas partie de MySql.Data (objet .NET)
        public CrudM()
        {
            InitializeComponent();
            afficher();
        }
        private void afficher()
        {
            try
            {
                _adapter = new MySqlDataAdapter("SELECT * FROM quizs;", _connexion); // Instancie un adapter
                _dt = new DataTable();  // Instancie une DataTable
                _adapter.Fill(_dt);     // Adapter remplit la DataTable
                dgQuiz.ItemsSource = _dt.DefaultView;  // Transfère les données de _dt vers dgRappeurs
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void effacer()
        {
            txtId.Content = "";
            SAI_rappeur_id.Text = "";
            SAI_question.Text = "";
            SAI_reponse1.Text = "";
            SAI_reponse2.Text = "";
            SAI_reponse3.Text = "";
            SAI_reponse4.Text = "";
            SAI_reponse.Text = "";

        }

        private void btnAjouter_Click(object sender, RoutedEventArgs e)
        {
            string _sql = "INSERT INTO quizs(rappeur_id, question, reponse1, reponse2, reponse3, reponse4, reponse) VALUES(@rappeur_id, @question, @reponse1, @reponse2, @reponse3,  @reponse4, @reponse)";

            try
            {
                _command = new MySqlCommand(_sql, _connexion);
                _command.Parameters.AddWithValue("@rappeur_id", decimal.Parse(SAI_rappeur_id.Text));
                _command.Parameters.AddWithValue("@question", SAI_question.Text);
                _command.Parameters.AddWithValue("@reponse1", SAI_reponse1.Text);
                _command.Parameters.AddWithValue("@reponse2", SAI_reponse2.Text);
                _command.Parameters.AddWithValue("@reponse3", SAI_reponse3.Text);
                _command.Parameters.AddWithValue("@reponse4", SAI_reponse4.Text);
                _command.Parameters.AddWithValue("@reponse", SAI_reponse.Text);


                _connexion.Open();
                _command.ExecuteNonQuery();

                afficher();
                effacer();

                MessageBox.Show("Quiz enregistré avec succès", "Nouveau quiz", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _command.Dispose();
                _connexion.Close();
            }


        }

        private void btnModifier_Click(object sender, RoutedEventArgs e)
        {
            string _sql = "UPDATE quizs SET rappeur_id = @rappeur_id, question = @question, reponse1 = @reponse1, reponse2 = @reponse2, reponse3 = @reponse3, reponse4 = @reponse4, reponse = @reponse WHERE id = @Id";
            try
            {
                _command = new MySqlCommand(_sql, _connexion);
                _command.Parameters.AddWithValue("@rappeur_id", txtId.Content);
                _command.Parameters.AddWithValue("@question", SAI_question.Text);
                _command.Parameters.AddWithValue("@reponse1", SAI_question.Text);
                _command.Parameters.AddWithValue("@reponse2", SAI_reponse2.Text);
                _command.Parameters.AddWithValue("@reponse3", SAI_reponse3.Text);
                _command.Parameters.AddWithValue("@reponse4", SAI_reponse4.Text);
                _command.Parameters.AddWithValue("@reponse", SAI_question.Text);

                _connexion.Open();
                _command.ExecuteNonQuery();

                afficher();

                MessageBox.Show("Quiz modifié", "changer Quiz", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _command.Dispose();
                _connexion.Close();
            }
        }

        private void btnSupprimer_Click(object sender, RoutedEventArgs e)
        {

            string _sql = "DELETE FROM quizs WHERE id = @Id;";

            try
            {
                _command = new MySqlCommand(_sql, _connexion);
                _command.Parameters.AddWithValue("@Id", txtId.Content);
                _connexion.Open();
                _command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _command.Dispose();
                _connexion.Close();
            }

            afficher();
            effacer();
        }

        private void dgQuiz_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if ((DataRowView)dgQuiz.SelectedItem != null)
            {
                DataRowView _drv = (DataRowView)dgQuiz.SelectedItem;

                txtId.Content = _drv.Row["id"].ToString();  // Supposant que 'id' est le champ pour l'identifiant dans votre base de données
                SAI_nom.Text = _drv.Row["rappeur_id"].ToString();
                SAI_note_globale.Text = _drv.Row["question"].ToString();
                SAI_vignette.Text = _drv.Row["reponse1"].ToString();
                SAI_picture.Text = _drv.Row["reponse2"].ToString();
                SAI_lien.Text = _drv.Row["reponse3"].ToString();
                SAI_musique.Text = _drv.Row["reponse4"].ToString();
                SAI_musique.Text = _drv.Row["reponse"].ToString();

            }
        }


    }
}

