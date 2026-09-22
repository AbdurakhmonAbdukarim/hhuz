namespace hhuz.Models;

public class AccessRules
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string RuleType { get; set; }
    public string RuleName { get; set; }



    //1.one side connection with Position
    public string PositionId{ get; set; }
    public Positions Position { get; set; }
}