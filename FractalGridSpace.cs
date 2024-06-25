using Godot;

public partial class FractalGridSpace : FractalSpace<Vector3>
{
	public override string GetNodeName(Vector3 coordinates)
	{
		return $"Node {coordinates.Z}:({coordinates.X}, {coordinates.Y})";
	}

	public Vector2 BottomLeft(Vector3 coordinates)
	{
		var offset = Mathf.Pow(4, (int)(coordinates.Z/2 + 1)) / 3;
		var size = Size(coordinates);
		return new Vector2(coordinates.X * size - offset, coordinates.Y * size - offset);
	}

	public int Size(Vector3 coordinates)
	{
		return (int)Mathf.Pow(2, coordinates.Z);
	}

	public Vector2 FindSquareAtLevel(Vector3 coordinates)
	{
		var bottomLeft = BottomLeft(coordinates);
		var size = Size(coordinates);
		var dx = coordinates.X - bottomLeft.X;
		var dy = coordinates.Y - bottomLeft.Y;
		return new Vector2((int)(dx / size), (int)(dy / size));
	}

	public override bool Contains(Vector3 ancestor, Vector3 descendant)
	{
		if(ancestor.Z < descendant.Z)
		{
			return false;
		}
		var ancestorSquare = FindSquareAtLevel(ancestor);
		var descendantSquare = FindSquareAtLevel(new Vector3(descendant.X, descendant.Y, ancestor.Z));
		return ancestorSquare == descendantSquare;
	}

	public override Vector3[] GetChildrenCoordinates(Vector3 coordinates)
	{
		if(coordinates.Z % 2 == 0)
		{
			return new Vector3[]
			{
				new Vector3(coordinates.X * 2, coordinates.Y * 2, coordinates.Z - 1),
				new Vector3(coordinates.X * 2 + 1, coordinates.Y * 2, coordinates.Z - 1),
				new Vector3(coordinates.X * 2, coordinates.Y * 2 + 1, coordinates.Z - 1),
				new Vector3(coordinates.X * 2 + 1, coordinates.Y * 2 + 1, coordinates.Z - 1)
			};
		}
		else
		{
			return new Vector3[]
			{
				new Vector3(coordinates.X * 2 - 1, coordinates.Y * 2 - 1, coordinates.Z - 1),
				new Vector3(coordinates.X * 2, coordinates.Y * 2 - 1, coordinates.Z - 1),
				new Vector3(coordinates.X * 2 - 1, coordinates.Y * 2, coordinates.Z - 1),
				new Vector3(coordinates.X * 2, coordinates.Y * 2, coordinates.Z - 1)
			};
		}
	}

	public override Vector3 GetParentCoordinates(Vector3 coordinates)
	{
		if(coordinates.Z % 2 == 0)
		{
			return new Vector3(
				(int)((coordinates.X + 1) / 2),
				(int)((coordinates.Y + 1) / 2),
				coordinates.Z + 1
			);
		}
		else
		{
			return new Vector3(
				(int)(coordinates.X / 2),
				(int)(coordinates.Y / 2),
				coordinates.Z + 1
			);
		}
	}

	public override bool IsOrigin(Vector3 coordinates)
	{
		return coordinates.X == 0 && coordinates.Y == 0;
	}
}
