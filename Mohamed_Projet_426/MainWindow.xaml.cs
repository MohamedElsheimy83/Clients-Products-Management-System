using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mohamed_Projet_426
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }//

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //appeler une methode pour afficher les listes des employes et des commandes
            this.Afficher();
        }//

        //Methode pour afficher les listes des employes et des commandes(qui existent dans les BD) dans la listView d'employe et de commandes
        private void Afficher()
        {
            using (dbSQLEntities entity = new dbSQLEntities())
            {
                ListViewEmployes.ItemsSource = entity.Employes.ToList();
                ListViewCommandes.ItemsSource = entity.Commandes.ToList();
            }
        }//

        //button qui efface les contenus des champs
        private void btnEffacer_Click(object sender, RoutedEventArgs e)
        {
            txtNom.Text = "";
            txtPrenom.Text = "";
            txtAdresse.Text = "";
            txtTitre.Text = "";
            txtProvince.Text = "";
            txtTelephone.Text = "";
            txtCodePostal.Text = "";
            txtExtension.Text = "";
            txtDateNaissance.Text = "";
            txtDateEmbauche.Text = "";
            txtPays.Text = "";
            txtNotes.Text = "";
            //désélectionner l'élément sélectionné
            ListViewEmployes.SelectedItem= null;
        }//

        //button qui ajoute un employe a la BD et la listView
        private void btnSauvegarder_Click(object sender, RoutedEventArgs e)
        {
            //verifier si les champs sont vides et les dates sont valides
            if (verifierChamps())
            {
                try
                {
                    // Instanciation d'un nouvel employe avec les donnees existant dans les champs
                    Employe employeAjoute = new Employe
                    {
                        Nom = txtNom.Text,
                        Prenom = txtPrenom.Text,
                        Adresse = txtAdresse.Text,
                        Titre = txtTitre.Text,
                        Province = txtProvince.Text,
                        Telephone = txtTelephone.Text,
                        CodePostal = txtCodePostal.Text,
                        Extension = txtExtension.Text,
                        DateDeNaissance = txtDateNaissance.SelectedDate,
                        DateEmbauche = txtDateEmbauche.SelectedDate,
                        Pays = txtPays.Text,
                        Notes = txtNotes.Text
                    };
                    using (dbSQLEntities entity = new dbSQLEntities())
                    {
                        //Ajouter le nouvel employe a la liste d'employes dans le BD
                        entity.Employes.Add(employeAjoute);
                        int res = entity.SaveChanges();
                        //appeler une methode pour afficher les listes des employes et des commandes
                        this.Afficher();
                        //efface les contenus des champs
                        btnEffacer_Click(sender, e);
                    }

                    MessageBox.Show("L'employé a été ajouté à la liste des employé!", "Info");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Impossible d'insérer cet employé, merci de revérifier tous les champs!");
                }
            }//end if       
        }//

        //Methode pour verifier si les champs sont vides et les dates sont valides
        private bool verifierChamps()
        {
            //condition pour vérifier si un des champs est vide (sauf les notes qui sont optionnelles)
            bool champsVide = (txtNom.Text == ""||txtPrenom.Text ==""||txtDateNaissance.Text ==""||txtAdresse.Text == ""||txtTitre.Text == ""||txtDateEmbauche.Text==""||txtProvince.Text == ""||txtTelephone.Text ==""|| txtPays.Text ==""|| txtCodePostal.Text ==""|| txtExtension.Text=="");
            //condition pour vérifier si la date du naissance est inférieure à 18 ou supérieur à 100
            bool dateNaissanceVer = false;
            //condition pour vérifier si la date d'embauche est supérieur à la date du naissance
            bool dateEmbaucheVer = false;

            bool champsVerification = false;

            if (champsVide)
            {
                MessageBox.Show("Veuillez compléter tous les champs s'il vous plaît!", "Avertissement");
            }

            if (!champsVide)
            {
                //vérifier si la date du naissance est inférieure à 18 ou supérieur à 100
                 dateNaissanceVer = ((DateTime.Now.Year - txtDateNaissance.SelectedDate.Value.Year) < 18) || ((DateTime.Now.Year - txtDateNaissance.SelectedDate.Value.Year) > 100);
                if(dateNaissanceVer)
                    MessageBox.Show("Impossible d'insérer cette date du naissance!", "Avertissement");
            }

            if (!champsVide && !dateNaissanceVer )
            {
                //vérifier si la date d'embauche est supérieur à la date du naissance et la difference et plus de 18 ans
                 dateEmbaucheVer = (txtDateNaissance.SelectedDate.Value.Year > txtDateEmbauche.SelectedDate.Value.Year) || ((txtDateEmbauche.SelectedDate.Value.Year-txtDateNaissance.SelectedDate.Value.Year)<18);
                if (dateEmbaucheVer)
                    MessageBox.Show("Impossible d'insérer cette date d'embauche! ", "Avertissement");
            }

            if (!champsVide && !dateNaissanceVer && !dateEmbaucheVer)
                champsVerification = true;

            return champsVerification;
        }//

    //Button qui quitte l'application
        private void btnQuitter_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        //Button pour modifier les donnees d'un employe selectionne
        private void btnModifier_Click(object sender, RoutedEventArgs e)
        {
            using (dbSQLEntities entity = new dbSQLEntities())
            {
                //obtenir les donnees d'un employe selectionne et les afficher dans les champs de la listeView Employe
                Employe selectedEmploye = (Employe)ListViewEmployes.SelectedItem;
                // verifier si un employe est selectionne
                if (selectedEmploye != null)
                {
                    if (verifierChamps()) {
                        //appeler une methode pour modifier les donnees d'un employe selectionne
                        Modifier(selectedEmploye.EmployeID);
                    
                    }//if
                }//if
                else
                {
                    MessageBox.Show("veuillez sélectionner un employé! ", "Avertissement");
                }//else
            }//using
        }
        //Methode pour modifier les donnees d'un employe 
        private void Modifier(int employeID)
        {
            dbSQLEntities entity = new dbSQLEntities();
            //modifier les donnees d'un employe selectionee dans la BD en cherchant l'employe par ID
            Employe chercheEmploye = entity.Employes.SingleOrDefault(e => e.EmployeID == employeID);
            if (chercheEmploye != null)
            {
                try
                {
                    employeID = chercheEmploye.EmployeID;
                    chercheEmploye.Nom = txtNom.Text;
                    chercheEmploye.Prenom = txtPrenom.Text;
                    chercheEmploye.Adresse = txtAdresse.Text;
                    chercheEmploye.Titre = txtTitre.Text;
                    chercheEmploye.Province = txtProvince.Text;
                    chercheEmploye.Telephone = txtTelephone.Text;
                    chercheEmploye.CodePostal = txtCodePostal.Text;
                    chercheEmploye.Extension = txtExtension.Text;
                    chercheEmploye.DateDeNaissance = txtDateNaissance.SelectedDate;
                    chercheEmploye.DateEmbauche = txtDateEmbauche.SelectedDate;
                    chercheEmploye.Pays = txtPays.Text;
                    chercheEmploye.Notes = txtNotes.Text;
                    //sauvegarder les modifications
                    int res = entity.SaveChanges();
                    MessageBox.Show("La modification a été effectuée avec succès! ", "Avertissement");
                    //effacer les contenus des champs
                    btnEffacer_Click(null, null);
                    //appeler une methode pour afficher les listes des employes et des commandes
                    this.Afficher();
                }
                catch (Exception ex) { 
                MessageBox.Show("Impossible de faire cette modification, merci de revérifier les champs modifiés!");
                }
            }//if

            }
        //Methode pour supprimer un employe selectionee de la liste et la BD
        private void btnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            //obtenir les donnees d'un employe selectionne et les afficher dans les champs de la liste
            Employe selectedEmploye = (Employe)ListViewEmployes.SelectedItem;
            //verifier si un employe est selectionee
            if (selectedEmploye == null) { 
                MessageBox.Show("veuillez sélectionner un employé! ", "Avertissement");
            return;
            }//if

            try
            {
                if(MessageBox.Show("Voulez vous supprimer cet employé ? ", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    using(dbSQLEntities entity  = new dbSQLEntities())
                    {
                        //supprimer l'employe de la BD

                        //Employe chercheEmploye = entity.Employes.SingleOrDefault(emp => emp.EmployeID == selectedEmploye.EmployeID);

                        Employe chercheEmploye = entity.Employes.Include(emp => emp.Commandes).SingleOrDefault(emp => emp.EmployeID == selectedEmploye.EmployeID);

                        entity.Employes.Remove(chercheEmploye);
                        //sauvegarder la modification(supprission)
                        int result = entity.SaveChanges();

                        if (result > 0) // l'employé a été supprimé
                        {
                         MessageBox.Show("L'employé a été supprimé", "Confirmation", MessageBoxButton.OK);
                        }
                        //afficher la nouvelle liste d'employes
                        this.Afficher();
                        //effacer le contenus de champs
                        btnEffacer_Click(sender, e);
                    }
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //Afficher les donnees d'un employee selectionee dans la listeViewEmploye et les commandes associes a cet employe
        private void lstEmployes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Employe selectedEmploye = (Employe)ListViewEmployes.SelectedItem;

                if(selectedEmploye != null)
                {
                    txtNom.Text = selectedEmploye.Nom;
                    txtPrenom.Text = selectedEmploye.Prenom;
                    txtAdresse.Text = selectedEmploye.Adresse;
                    txtTitre.Text = selectedEmploye.Titre;
                    txtProvince.Text = selectedEmploye.Province;
                    txtTelephone.Text = selectedEmploye.Telephone;
                    txtCodePostal.Text = selectedEmploye.CodePostal;
                    txtExtension.Text = selectedEmploye.Extension;
                    txtDateNaissance.SelectedDate = selectedEmploye.DateDeNaissance;
                    txtDateEmbauche.SelectedDate = selectedEmploye.DateEmbauche;
                    txtPays.Text = selectedEmploye.Pays;
                    txtNotes.Text = selectedEmploye.Notes;
                    //afficher les commandes associes a un employe selectionee dans la listViewCommandes
                    ListViewCommandes.ItemsSource = getQueryListCommandes(selectedEmploye.EmployeID);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //Methode pour obtenir les commandes associes a un employe selectionee 
        private IEnumerable getQueryListCommandes(int employeID)
        {
            try
            {
                using (dbSQLEntities entity = new dbSQLEntities())
                {
                    return entity.Commandes.Where(c => c.EmployeID == employeID).ToList();
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message);
                return null;
            }
        }
        //Afficher les donnees d'un client qui a fait une commande selectionee
        private void lstCommandes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Commande selectedCommande = (Commande)ListViewCommandes.SelectedItem;
              
                if (selectedCommande != null)
                {
                    IEnumerable liste_Clients = getQueryListClients(selectedCommande.ClientID);          
                    ListeClients list = new ListeClients(liste_Clients) ;
                    list.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //Methode pour obtenir les donnees associes a un client pour un commande selectionee 
        private IEnumerable getQueryListClients(string clientID)
        {
            try
            {
                using (dbSQLEntities entity = new dbSQLEntities())
                {
                    return entity.Clients.Where(c => c.ClientID == clientID).ToList();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        //quitter l'application
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
