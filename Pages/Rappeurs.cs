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
                _adapter = new MySqlDataAdapter("SELECT * FROM rappeurs;", _connexion); // Instancie un adapter
                _dt = new DataTable();  // Instancie une DataTable
                _adapter.Fill(_dt);     // Adapter remplit la DataTable
                dgRappeurs.ItemsSource = _dt.DefaultView;  // Transfère les données de _dt vers dgRappeurs
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void effacer()
        {
            txtId.Content = "";
            SAI_nom.Text = "";
            SAI_note_globale.Text = "";
            SAI_vignette.Text = "";
            SAI_picture.Text = "";
            SAI_lien.Text = "";
            SAI_musique.Text = "";

        }

        private void btnAjouter_Click(object sender, RoutedEventArgs e)
        {
            string _sql = "INSERT INTO rappeurs(nom, note_globale, vignette, picture, lien, musique) VALUES(@nom, @note_globale, @vignette, @picture, @lien,  @musique)";

            try
            {
                _command = new MySqlCommand(_sql, _connexion);
                _command.Parameters.AddWithValue("@nom", SAI_nom.Text);
                _command.Parameters.AddWithValue("@note_globale", decimal.Parse(SAI_note_globale.Text));
                _command.Parameters.AddWithValue("@vignette", SAI_vignette.Text);
                _command.Parameters.AddWithValue("@picture", SAI_picture.Text);
                _command.Parameters.AddWithValue("@lien", SAI_lien.Text);
                _command.Parameters.AddWithValue("@musique", SAI_musique.Text);


                _connexion.Open();
                _command.ExecuteNonQuery();

                afficher();
                effacer();

                MessageBox.Show("Article enregistré avec succès", "Nouvel article", MessageBoxButton.OK, MessageBoxImage.Information);
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
            string _sql = "UPDATE rappeurs SET nom = @nom, note_globale = @note_globale, vignette = @vignette, picture = @picture, lien = @lien, musique = @musique WHERE id = @Id";
            try
            {
                _command = new MySqlCommand(_sql, _connexion);
                _command.Parameters.AddWithValue("@Id", txtId.Content);
                _command.Parameters.AddWithValue("@nom", SAI_nom.Text);
                _command.Parameters.AddWithValue("@note_globale", decimal.Parse(SAI_note_globale.Text));
                _command.Parameters.AddWithValue("@vignette", SAI_vignette.Text);
                _command.Parameters.AddWithValue("@picture", SAI_picture.Text);
                _command.Parameters.AddWithValue("@lien", SAI_lien.Text);
                _command.Parameters.AddWithValue("@musique", SAI_musique.Text);

                _connexion.Open();
                _command.ExecuteNonQuery();

                afficher();

                MessageBox.Show("Article modifier avec succès", "changer Article", MessageBoxButton.OK, MessageBoxImage.Information);
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

            string _sql = "DELETE FROM rappeurs WHERE id = @Id;";

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

        private void dgRappeurs_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if ((DataRowView)dgRappeurs.SelectedItem != null)
            {
                DataRowView _drv = (DataRowView)dgRappeurs.SelectedItem;

                txtId.Content = _drv.Row["id"].ToString();  // Supposant que 'id' est le champ pour l'identifiant dans votre base de données
                SAI_nom.Text = _drv.Row["nom"].ToString();
                SAI_note_globale.Text = _drv.Row["note_globale"].ToString();
                SAI_vignette.Text = _drv.Row["vignette"].ToString();
                SAI_picture.Text = _drv.Row["picture"].ToString();
                SAI_lien.Text = _drv.Row["lien"].ToString();
                SAI_musique.Text = _drv.Row["musique"].ToString();

            }
        }


    }
}
