using System.Text;

namespace TpMath2
{
    public class Automate
    {
        // État initial de l'automate (le point de départ).
        public State InitialState { get; private set; }

        // État courant de l'automate (celui dans lequel on se trouve après les transitions).
        public State CurrentState { get; private set; }

        // Liste de tous les états de l'automate.
        public List<State> States { get; private set; }

        // Indique si l'automate est valide (respecte les critères imposés).
        public bool IsValid { get; private set; }

        // Constructeur qui initialise un automate à partir d'un fichier donné.
        public Automate(string filePath)
        {
            States = new List<State>(); // Initialise la liste des états.
            LoadFromFile(filePath); // Charge l'automate depuis le fichier.
        }

        // Méthode pour charger l'automate à partir d'un fichier.
        private void LoadFromFile(string filePath)
        {
            try
            {
                // Lire toutes les lignes du fichier.
                var lines = File.ReadAllLines(filePath);

                foreach (var line in lines)
                {
                    // Ignorer les lignes vides.
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    // Découper chaque ligne en parties (action + arguments).
                    var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    if (parts[0] == "state")
                    {
                        // Ajouter un état à l'automate.
                        string name = parts[1];
                        bool isFinal = parts[2] == "1"; // Indique si l'état est final.
                        bool isInitial = parts[3] == "1"; // Indique si l'état est initial.

                        var state = new State(name, isFinal);
                        States.Add(state);

                        // Si l'état est initial, vérifier qu'il n'y en a pas déjà un.
                        if (isInitial)
                        {
                            Console.WriteLine($"État initial trouvé : {name}");
                            if (InitialState != null)
                            {
                                // Automate invalide si plusieurs états initiaux.
                                Console.WriteLine($"Erreur : Plus d'un état initial détecté ({name}).");
                                IsValid = false;
                                return;
                            }
                            InitialState = state;
                        }
                    }
                    else if (parts[0] == "transition")
                    {
                        // Ajouter une transition entre deux états.
                        string from = parts[1]; // État source.
                        char input = parts[2][0]; // Symbole d'entrée.
                        string to = parts[3]; // État cible.

                        var fromState = States.FirstOrDefault(s => s.Name == from);
                        var toState = States.FirstOrDefault(s => s.Name == to);

                        if (fromState != null && toState != null)
                        {
                            // Ajouter la transition si les états existent.
                            fromState.Transitions.Add(new Transition(input, toState));
                        }
                        else
                        {
                            // Transition invalide si l'un des états n'existe pas.
                            Console.WriteLine($"Erreur : Transition invalide ({line}). État(s) introuvable(s).");
                        }
                    }
                    else
                    {
                        // Ignorer les lignes qui ne correspondent pas à une action valide.
                        Console.WriteLine($"Ligne ignorée : {line}");
                    }
                }

                // Vérifier qu'un état initial est défini.
                if (InitialState == null)
                {
                    Console.WriteLine("Erreur : Aucun état initial défini.");
                    IsValid = false;
                    return;
                }

                // Vérifier qu'au moins un état est défini.
                if (!States.Any())
                {
                    Console.WriteLine("Erreur : Aucun état défini dans l'automate.");
                    IsValid = false;
                    return;
                }

                // Vérification de non-déterminisme.
                foreach (var state in States)
                {
                    // Identifier les entrées ayant plusieurs transitions.
                    var duplicateInputs = state.Transitions
                        .GroupBy(t => t.Input)
                        .Where(g => g.Count() > 1)
                        .Select(g => g.Key);

                    if (duplicateInputs.Any())
                    {
                        // Automate invalide si plusieurs transitions pour une même entrée.
                        Console.WriteLine($"Erreur : Automate non déterministe détecté dans l'état {state.Name} pour les entrées {string.Join(", ", duplicateInputs)}.");
                        IsValid = false;
                        return;
                    }
                }

                // Si toutes les vérifications passent, l'automate est valide.
                IsValid = true;
            }
            catch (Exception ex)
            {
                // Gestion des erreurs lors de la lecture du fichier.
                Console.WriteLine($"Erreur lors du chargement du fichier : {ex.Message}");
                IsValid = false;
            }
        }

        // Valide une chaîne d'entrée selon les transitions de l'automate.
        public bool Validate(string input)
        {
            bool isValid = true; // Supposer que l'entrée est valide au départ.
            Reset(); // Réinitialiser l'état courant à l'état initial.

            // Parcourir chaque caractère de la chaîne d'entrée.
            foreach (char c in input)
            {
                // Chercher la transition correspondant à l'entrée.
                var transition = CurrentState.Transitions.FirstOrDefault(t => t.Input == c);

                if (transition == null)
                {
                    // Aucune transition trouvée pour l'entrée actuelle.
                    Console.WriteLine($"Rejeté : Pas de transition pour '{c}' depuis l'état {CurrentState.Name}.");
                    isValid = false;
                    break;
                }

                // Afficher la transition effectuée.
                Console.WriteLine($"Depuis {CurrentState.Name}, lu : {c}, transite vers {transition.TransiteTo.Name}");

                // Mettre à jour l'état courant.
                CurrentState = transition.TransiteTo;
            }

            // Vérifier si l'état final atteint est un état final de l'automate.
            if (isValid && !CurrentState.IsFinal)
            {
                Console.WriteLine($"Rejeté : L'état final atteint ({CurrentState.Name}) n'est pas un état final.");
                isValid = false;
            }

            // Afficher le résultat final.
            if (isValid)
            {
                Console.WriteLine($"Accepté : La chaîne est valide et atteint l'état final ({CurrentState.Name}).");
            }

            return isValid;
        }

        // Réinitialiser l'état courant à l'état initial.
        public void Reset()
        {
            if (InitialState != null)
            {
                CurrentState = InitialState;
            }
            else
            {
                // Erreur si aucun état initial n'est défini.
                Console.WriteLine("Erreur : Aucun état initial défini pour réinitialiser l'automate.");
            }
        }

        // Génère une représentation lisible de l'automate.
        public override string ToString()
        {
            var builder = new StringBuilder();

            // Identifier l'état initial.
            builder.AppendLine($"État initial : [{InitialState?.Name ?? "Non défini"}]");

            // Parcourir tous les états et leurs transitions.
            foreach (var state in States)
            {
                // Ajouter les crochets pour l'état initial et les parenthèses pour les états finaux.
                string stateName = state.Name;
                if (state == InitialState)
                {
                    stateName = $"[{state.Name}]";
                }
                if (state.IsFinal)
                {
                    stateName = $"({stateName})";
                }
                builder.AppendLine(stateName);

                // Ajouter les transitions pour l'état courant.
                if (state.Transitions.Any())
                {
                    foreach (var transition in state.Transitions)
                    {
                        builder.AppendLine($"  --{transition.Input}--> {transition.TransiteTo.Name}");
                    }
                }
                else
                {
                    // Indiquer si l'état n'a aucune transition.
                    builder.AppendLine("  (aucune transition)");
                }
            }

            return builder.ToString();
        }
    }
}

