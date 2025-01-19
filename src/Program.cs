
namespace TpMath2
{
    //Entrée du programme
    public class Program
    {
        static void Main(string[] args)
        {
            // Étape 1 : Affichage de l'entête
            Console.WriteLine("=== Application Automate ===");
            Console.WriteLine("Nom et prénom: Salifou Zéinab, (Code Permanent: SALZ22339800)");
            Console.WriteLine("=============================");

            Automate automate = null;// Déclaration de l'objet automate, initialisé à null

            while (true)
            {
                // Saut de ligne pour séparer l'action précédente du menu
                Console.WriteLine();

                // Menu principal
                Console.WriteLine("Menu:");
                Console.WriteLine("1. Charger un fichier automate");
                Console.WriteLine("2. Afficher la structure de l'automate");
                Console.WriteLine("3. Valider une chaîne d'entrée");
                Console.WriteLine("4. Quitter");
                Console.Write("Choisissez une option : ");
                string choix = Console.ReadLine();

                // Saut de ligne pour séparer l'option choisie de l'action suivante
                Console.WriteLine();

                switch (choix)
                {
                    case "1":
                        // Étape 2 : Charger un fichier automate
                        Console.Write("Entrez le chemin du fichier automate : ");
                        string filePath = Console.ReadLine();
                        Console.WriteLine(); // Saut de ligne après l'entrée de l'utilisateur

                        try
                        {
                            // Tentative de chargement de l'automate depuis le fichier
                            automate = new Automate(filePath);

                            // Vérification de la validité de l'automate
                            if (!automate.IsValid)
                            {
                                Console.WriteLine("Automate invalide. L'application va se fermer.");
                                Console.ReadLine();
                                return;// Fermeture du programme si l'automate est invalide
                            }
                            Console.WriteLine("Automate chargé avec succès !");
                        }
                        catch (Exception ex)
                        {
                            // Gestion des erreurs en cas de problème lors du chargement
                            Console.WriteLine($"Erreur lors du chargement : {ex.Message}");
                        }
                        break;

                    case "2":
                        // Étape 3 : Afficher la structure de l'automate
                        if (automate == null)
                        {
                            // Message d'erreur si l'automate ne s'affiche pas
                            Console.WriteLine("Aucun automate chargé. Veuillez charger un automate d'abord.");
                        }
                        else
                        {
                            // Afficher la structure de l'automate si chargé
                            Console.WriteLine("Structure de l'automate :");
                            Console.WriteLine(automate);
                        }
                        break;

                    case "3":
                        // Étape 4 : Valider une chaîne d'entrée
                        if (automate == null)
                        {
                            // Message d'erreur si aucun automate n'a été chargé
                            Console.WriteLine("Aucun automate chargé. Veuillez charger un automate d'abord.");
                        }
                        else
                        {
                            // Demande d'une chaîne d'entrée
                            Console.Write("Entrez une chaîne de 0 et 1 (ou 'exit' pour revenir au menu) : ");
                            string input = Console.ReadLine();
                            Console.WriteLine(); // Saut de ligne après l'entrée de l'utilisateur

                            if (input.ToLower() == "exit")
                                break;// Retour au menu si l'utilisateur entre "exit"

                            if (!IsValidInput(input))
                            {
                                // Message d'erreur si la chaîne contient des caractères invalides
                                Console.WriteLine("Chaîne invalide. Seuls les caractères '0' et '1' sont autorisés.");
                            }
                            else
                            {
                                // Validation de la chaîne via l'automate
                                Console.WriteLine("Validation de la chaîne...");
                                automate.Validate(input);
                            }
                        }
                        break;

                    case "4":
                        // Étape 5 : Quitter l'application
                        Console.WriteLine("Fermeture de l'application. Appuyez sur ENTER pour quitter.");
                        Console.ReadLine();
                        return;

                    default:
                        // Message pour les choix invalides
                        Console.WriteLine("Option invalide. Veuillez réessayer.");
                        break;
                }
            }
        }

        /// <summary>
        /// Vérifie si une chaîne est composée uniquement de '0' et '1'.
        /// </summary>
        /// <param name="input">La chaîne à valider</param>
        /// <returns>True si valide, False sinon</returns>
        private static bool IsValidInput(string input)
        {
            // Parcourt chaque caractère de la chaîne pour vérifier s'il est '0' ou '1'
            foreach (char c in input)
            {
                if (c != '0' && c != '1')
                {
                    return false;// Retourne false si un caractère invalide est trouvé
                }
            }
            return true;// Retourne true si tous les caractères sont valides
        }
    }
}

