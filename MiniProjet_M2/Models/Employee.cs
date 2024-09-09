namespace MiniProjet_M2.Models;

// classe du model Employee
public class Employee
{
    public int Id { get; set; }
    public double CurrentMotivation { get; set; } // motivation courant
    public double MotivationA1 { get; set; } // motivation apres action 1
    public double MotivationA2 { get; set; } // motivation apres action 2
    public double MotivationA3 { get; set; } // motivation apres action 3
    public double MotivationA4 { get; set; } // motivation apres action 4

    public override string ToString()
    {
        return $"Id: {Id}, CurrentMotivation: {CurrentMotivation}";
    }
}