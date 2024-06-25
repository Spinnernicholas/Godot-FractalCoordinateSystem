using Godot;

public partial class FractalController : Node
{
	[Export]
	public FractalGridSpace GridSpace { get; set; }

	public override void _Ready()
	{
		GridSpace.AddFractalNode(new Vector3(0, 0, 0), new Node(){Name = "Node 1"});
		GridSpace.AddFractalNode(new Vector3(2, 5, 5), new Node(){Name = "Node 2"});
	}
}
