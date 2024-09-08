namespace MiniProjet_M2.Models;

public class Employee
{
    public int Id { get; set; }
    public double CurrentMotivation { get; set; }
    public double MotivationA1 { get; set; }
    public double MotivationA2 { get; set; }
    public double MotivationA3 { get; set; }
    public double MotivationA4 { get; set; }
    
    public override string ToString()
    {
        return $"Id: {Id}, CurrentMotivation: {CurrentMotivation}";
    }
}