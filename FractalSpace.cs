using Godot;
using System;
using System.Collections.Generic;

public partial class FractalNode<T> : Node
{
    public FractalSpace<T> ParentGrid { get; set; }
    public T Coordinates { get; set; }
}

public abstract partial class FractalSpace<T> : Node
{
    private Dictionary<T, FractalNode<T>> fractalNodes = new Dictionary<T, FractalNode<T>>();

    public abstract T GetParentCoordinates(T coordinates);
    public abstract T[] GetChildrenCoordinates(T coordinates);
    public abstract bool IsOrigin(T coordinates);
    public abstract bool Contains(T ancestor, T descendant);
    public abstract String GetNodeName(T coordinates);

    public void AddFractalNode(T coordinates, Node node)
    {
        GD.Print("Adding Fractal Node");
        if(fractalNodes.Count == 0)
        {
            var fractalNode = new FractalNode<T>
            {
                ParentGrid = this,
                Coordinates = coordinates,
                Name = GetNodeName(coordinates)
            };
            fractalNode.AddChild(node);
            fractalNodes.Add(coordinates, fractalNode);
            AddChild(fractalNode);
        }
        else if (fractalNodes.ContainsKey(coordinates))
        {
            fractalNodes[coordinates].AddChild(node);
        }
        else if (Contains(GetChild<FractalNode<T>>(0).Coordinates,coordinates))
        {
            FractalNode<T> currentNode = new FractalNode<T>
            {
                ParentGrid = this,
                Coordinates = coordinates,
                Name = GetNodeName(coordinates)
            };
            currentNode.AddChild(node);
            fractalNodes.Add(coordinates, currentNode);
            var parentCoordinates = GetParentCoordinates(coordinates);
            while(!fractalNodes.ContainsKey(parentCoordinates))
            {
                var fractalNode = new FractalNode<T>
                {
                    ParentGrid = this,
                    Coordinates = parentCoordinates,
                    Name = GetNodeName(coordinates)
                };
                fractalNode.AddChild(currentNode);
                fractalNodes.Add(parentCoordinates, fractalNode);
                parentCoordinates = GetParentCoordinates(parentCoordinates);
            }
            }
        else if (Contains(coordinates, GetChild<FractalNode<T>>(0).Coordinates))
        {
            FractalNode<T> currentNode = new FractalNode<T>
            {
                ParentGrid = this,
                Coordinates = coordinates,
                Name = GetNodeName(coordinates)
            };
            currentNode.AddChild(node);
            fractalNodes.Add(coordinates, currentNode);
            var parentCoordinates = GetParentCoordinates(coordinates);
            while(!fractalNodes.ContainsKey(parentCoordinates))
            {
                var fractalNode = new FractalNode<T>
                {
                    ParentGrid = this,
                    Coordinates = parentCoordinates,
                    Name = GetNodeName(coordinates)
                };
                fractalNode.AddChild(currentNode);
                fractalNodes.Add(parentCoordinates, fractalNode);
                parentCoordinates = GetParentCoordinates(parentCoordinates);
            }
        } else {
            GD.PushError("Coordinates are not within the grid");
        }
    }
}