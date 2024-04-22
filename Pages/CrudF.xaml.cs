﻿using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace wpf_application.Pages
{
    /// <summary>
    /// Logique d'interaction pour CrudF.xaml
    /// </summary>
    public partial class CrudF : Page
    {
        // Objets nécessaires pour SQL
        const string _dsn = "server=localhost;port=3306;database=projet_sneakers;username=root;password=;";
        private MySqlConnection _connexion = new MySqlConnection(_dsn);
        private MySqlCommand _command;
        private MySqlDataAdapter _adapter;

        private DataTable _dt;  // Ne fait pas partie de MySql.Data (objet .NET)
        public CrudF()
        {
            InitializeComponent();
            afficher();
        }
        private void afficher()
        {
            try
            {
                _adapter = new MySqlDataAdapter("SELECT * FROM familles;", _connexion); // Instancie un adapter
                _dt = new DataTable();  // Instancie une DataTable
                _adapter.Fill(_dt);     // Adapter remplit la DataTable
                dgFamilles.ItemsSource = _dt.DefaultView;  // Transfère les données de _dt vers dgArticles
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void effacer()
        {
            txtId.Content = "";
            SAI_NomFamille.Text = "";
            SAI_IdParent.Text = "";
            // Effacez les autres champs si nécessaire
        }

        private void btnAjouter_Click(object sender, RoutedEventArgs e)
        {
            string _sql = "INSERT INTO familles(nom_famille, id_parent) VALUES(@NomFamille, @IdParent)";
            try
            {
                _command = new MySqlCommand(_sql, _connexion);
                _command.Parameters.AddWithValue("@NomFamille", SAI_NomFamille.Text);
                _command.Parameters.AddWithValue("@IdParent", string.IsNullOrEmpty(SAI_IdParent.Text) ? (object)DBNull.Value : int.Parse(SAI_IdParent.Text));

                _connexion.Open();
                _command.ExecuteNonQuery();

                afficher();
                effacer();

                MessageBox.Show("Famille enregistrée avec succès", "Nouvelle famille", MessageBoxButton.OK, MessageBoxImage.Information);
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
            string _sql = "UPDATE familles SET nom_famille = @NomFamille, id_parent = @IdParent WHERE id = @Id";
            try
            {
                _command = new MySqlCommand(_sql, _connexion);
                _command.Parameters.AddWithValue("@Id", txtId.Content);
                _command.Parameters.AddWithValue("@NomFamille", SAI_NomFamille.Text);
                _command.Parameters.AddWithValue("@IdParent", string.IsNullOrEmpty(SAI_IdParent.Text) ? (object)DBNull.Value : int.Parse(SAI_IdParent.Text));

                _connexion.Open();
                _command.ExecuteNonQuery();

                afficher();

                MessageBox.Show("Famille modifiée avec succès", "Modifier Famille", MessageBoxButton.OK, MessageBoxImage.Information);
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
            string _sql = "DELETE FROM familles WHERE id = @Id;";
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

        private void dgArticles_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if ((DataRowView)dgFamilles.SelectedItem != null)
            {
                DataRowView _drv = (DataRowView)dgFamilles.SelectedItem;

                txtId.Content = _drv.Row["id"].ToString();
                SAI_NomFamille.Text = _drv.Row["nom_famille"].ToString();
                SAI_IdParent.Text = _drv.Row["id_parent"].ToString();
            }
        }

    }
}
