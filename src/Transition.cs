
namespace TpMath2
{
    // La classe Transition représente une transition dans un automate.
    // Une transition est définie par :
    // - Un symbole d'entrée (Input) : un caractère qui déclenche la transition.
    // - Un état cible (TransiteTo) : l'état vers lequel l'automate transite lorsque l'input est lu.

    public class Transition
    {
        // Symbole d'entrée qui déclenche la transition, comme '0' ou '1'.
        public char Input { get; set; }

        // L'état cible vers lequel cette transition conduit.
        public State TransiteTo { get; set; }

        // Constructeur de la classe Transition.
        // Initialise une transition avec un symbole d'entrée et un état cible.
        public Transition(char input, State transiteTo)
        {
            Input = input; // Symbole qui déclenche cette transition.
            TransiteTo = transiteTo; // État cible atteint par cette transition.
        }

        // Retourne une représentation lisible de la transition.
        // Exemple : "--0--> s1" pour une transition avec l'input '0' et l'état cible 's1'.
        public override string ToString()
        {
            // Format de la transition : "--<input>--> <nom de l'état cible>".
            return $"--{Input}--> {TransiteTo.Name}";
        }
    }
}
