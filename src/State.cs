using System.Text;

namespace TpMath2
{
    // La classe State représente un état dans un automate.
    // Un état a :
    // - Un nom (Name), par exemple : "s0", "s1".
    // - Une propriété indiquant s'il est final (IsFinal).
    // - Une liste de transitions (Transitions) vers d'autres états.

    public class State
    {
        // Indique si l'état est un état final.
        public bool IsFinal { get; private set; }

        // Nom unique de l'état (exemple : "s0").
        public string Name { get; private set; }

        // Liste des transitions depuis cet état.
        public List<Transition> Transitions { get; private set; }

        // Constructeur de la classe State.
        // Initialise l'état avec son nom et s'il est final.
        public State(string name, bool isFinal)
        {
            Name = name; // Nom de l'état.
            IsFinal = isFinal; // Indique si l'état est final.
            Transitions = new List<Transition>(); // Initialise une liste vide de transitions.
        }

        // Retourne une représentation lisible de l'état et de ses transitions.
        public override string ToString()
        {
            // Format de l'état : "[Nom]" avec "(final)" si c'est un état final.
            var result = new StringBuilder();
            result.Append($"{Name}{(IsFinal ? " (final)" : "")}");

            // Ajouter les transitions à la représentation de l'état.
            if (Transitions.Any())
            {
                result.AppendLine(" :"); // Marque le début de la liste des transitions.
                foreach (var transition in Transitions)
                {
                    // Ajoute chaque transition sous la forme : "--<input>--> <nom de l'état cible>".
                    result.AppendLine($"  --{transition.Input}--> {transition.TransiteTo.Name}");
                }
            }
            else
            {
                // Si l'état n'a aucune transition.
                result.AppendLine(" (aucune transition)");
            }

            return result.ToString(); // Retourne la chaîne décrivant l'état.
        }
    }
}
