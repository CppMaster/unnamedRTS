using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PathFinding
{

	[System.Serializable]
	public class IntVector2D
	{
		public int x = 0;
		public int y = 0;

		public IntVector2D(int _x, int _y)
		{
			x = _x;
			y = _y;
		}

		public static float Distance(IntVector2D a, IntVector2D b)
		{
			return Mathf.Sqrt((a.x-b.x)*(a.x-b.x) + (a.y-b.y)*(a.y-b.y));
		}

		public override string ToString ()
		{
			return string.Format ("[" + x + ":" + y + "]");
		}
	
	}

	class IntVector2DEqualityComparer : IEqualityComparer<IntVector2D>
	{
		
		public bool Equals(IntVector2D b1, IntVector2D b2)
		{
			if (b1.x == b2.x & b1.y == b2.y)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		
		public int GetHashCode(IntVector2D bx)
		{
			int hCode = bx.x ^ bx.y;
			return hCode.GetHashCode();
		}
		
	}

	public class GridNode
	{
		public float f = 0f;
		public float g = float.PositiveInfinity;
		public IntVector2D cameFrom = null;
	}

	public class Grid : MonoBehaviour {

		public int maxTries = 100;

		public int gridSize = 20;
		public int tileSize = 2;

		GridTile[,] tiles;

		[ContextMenu ("Awake")]
		void Awake()
		{
			tiles = new GridTile[gridSize,gridSize];
			for(int x = 0; x < gridSize; ++x)
			{
				for(int y = 0; y < gridSize; ++y)
				{
					tiles[x,y] = new GridTile();
				}
			}

			UpdateGrid();
		}

		[ContextMenu ("Update Grid")]
		void UpdateGrid()
		{

			SolidObstacle[] obstacles = GameObject.FindObjectsOfType<SolidObstacle>();
			foreach(SolidObstacle obs in obstacles)
			{
				Vector3 cornerPos = obs.transform.position - Vector3.one * obs.size / 2;
				int cornerPosX = Mathf.RoundToInt(cornerPos.x / tileSize);
				int cornerPosY = Mathf.RoundToInt(cornerPos.z / tileSize);
				for(int x = cornerPosX + gridSize / 2; x < cornerPosX + gridSize / 2 + obs.size / tileSize; ++x)
				{
					if(x < 0 || x >= gridSize) continue;
					for(int y = cornerPosY + gridSize / 2; y < cornerPosY + gridSize / 2 + obs.size / tileSize; ++y)
					{
						if(y < 0 || y >= gridSize) continue;
						tiles[x,y].blocked = true;
					}
				}
			}
		}

		void OnDrawGizmos()
		{
			if(tiles == null) return;
			/*for(int x = 0; x < gridSize; ++x)
			{
				for(int y = 0; y < gridSize; ++y)
				{
					if(tiles[x,y].blocked) continue;
					Gizmos.color = new Color(1,tiles[x,y].wage * 0.1f,0);
					Gizmos.DrawWireCube(
						new Vector3((x + 0.5f - gridSize / 2) * tileSize,tiles[x,y].wage * 0.1f
					            , (y + 0.5f - gridSize / 2) * tileSize)
								, new Vector3(tileSize, 0.1f, tileSize) * 0.9f);
				}
			}*/

		}

		List<IntVector2D> GetNeighbours(IntVector2D node, int mode)
		{
			List<IntVector2D> list = new List<IntVector2D>();
			switch(mode)
			{
				case 0:
				{
					{
						IntVector2D newNode = new IntVector2D(node.x - 1, node.y);
						if(CheckNode(newNode)) list.Add(newNode);
					}
					{
						IntVector2D newNode = new IntVector2D(node.x + 1, node.y);
						if(CheckNode(newNode)) list.Add(newNode);
					}
					{
						IntVector2D newNode = new IntVector2D(node.x, node.y + 1);
						if(CheckNode(newNode)) list.Add(newNode);
					}
					{
						IntVector2D newNode = new IntVector2D(node.x, node.y - 1);
						if(CheckNode(newNode)) list.Add(newNode);
					}
					break;
				}
				case 1:
				{
				bool[,] square = new bool[3,3];
				{
					IntVector2D newNode = new IntVector2D(node.x - 1, node.y);
					if(CheckNode(newNode))
					{	
						list.Add(newNode);
						square[0,1] = true;
					}
				}
				{
					IntVector2D newNode = new IntVector2D(node.x + 1, node.y);
					if(CheckNode(newNode))
					{	
						list.Add(newNode);
						square[2,1] = true;
					}
				}
				{
					IntVector2D newNode = new IntVector2D(node.x, node.y + 1);
					if(CheckNode(newNode))
					{	
						list.Add(newNode);
						square[1,2] = true;
					}
				}
				{
					IntVector2D newNode = new IntVector2D(node.x, node.y - 1);
					if(CheckNode(newNode))
					{	
						list.Add(newNode);
						square[1,0] = true;
					}
				}

				if(square[0,1] && square[1,0])
				{
					IntVector2D newNode = new IntVector2D(node.x - 1, node.y - 1);
					if(CheckNode(newNode))
					{	
						list.Add(newNode);
					}
				}
				if(square[2,1] && square[1,2])
				{
					IntVector2D newNode = new IntVector2D(node.x + 1, node.y + 1);
					if(CheckNode(newNode))
					{	
						list.Add(newNode);
					}
				}
				if(square[0,1] && square[1,2])
				{
					IntVector2D newNode = new IntVector2D(node.x - 1, node.y + 1);
					if(CheckNode(newNode))
					{	
						list.Add(newNode);
					}
				}
				if(square[2,1] && square[1,0])
				{
					IntVector2D newNode = new IntVector2D(node.x + 1, node.y - 1);
					if(CheckNode(newNode))
					{	
						list.Add(newNode);
					}
				}

				break;
				}

			case 2:
			{
				bool[,] square = new bool[5,5];
				{
					IntVector2D newNode = new IntVector2D(node.x - 1, node.y);
					if(CheckNode(newNode))
					{	
						list.Add(newNode);
						square[0,1] = true;
					}
				}
				{
					IntVector2D newNode = new IntVector2D(node.x + 1, node.y);
					if(CheckNode(newNode))
					{	
						list.Add(newNode);
						square[2,1] = true;
					}
				}
				{
					IntVector2D newNode = new IntVector2D(node.x, node.y + 1);
					if(CheckNode(newNode))
					{	
						list.Add(newNode);
						square[1,2] = true;
					}
				}
				{
					IntVector2D newNode = new IntVector2D(node.x, node.y - 1);
					if(CheckNode(newNode))
					{	
						list.Add(newNode);
						square[1,0] = true;
					}
				}
				
				if(square[0,1] && square[1,0])
				{
					IntVector2D newNode = new IntVector2D(node.x - 1, node.y - 1);
					if(CheckNode(newNode))
					{	
						list.Add(newNode);
						square[0,0] = true;
					}
				}
				if(square[2,1] && square[1,2])
				{
					IntVector2D newNode = new IntVector2D(node.x + 1, node.y + 1);
					if(CheckNode(newNode))
					{	
						list.Add(newNode);
						square[2,2] = true;
					}
				}
				if(square[0,1] && square[1,2])
				{
					IntVector2D newNode = new IntVector2D(node.x - 1, node.y + 1);
					if(CheckNode(newNode))
					{	
						list.Add(newNode);
						square[0,2] = true;
					}
				}
				if(square[2,1] && square[1,0])
				{
					IntVector2D newNode = new IntVector2D(node.x + 1, node.y - 1);
					if(CheckNode(newNode))
					{	
						list.Add(newNode);
						square[2,0] = true;
					}
				}

				if(square[0,2] && square[1,2])
				{
					IntVector2D newNode = new IntVector2D(node.x - 1, node.y + 2);
					if(CheckNode(newNode))
					{	
						list.Add(newNode);
					}
				}
				if(square[2,2] && square[1,2])
				{
					IntVector2D newNode = new IntVector2D(node.x + 1, node.y + 2);
					if(CheckNode(newNode))
					{	
						list.Add(newNode);
					}
				}
				if(square[2,2] && square[2,1])
				{
					IntVector2D newNode = new IntVector2D(node.x + 2, node.y + 1);
					if(CheckNode(newNode))
					{	
						list.Add(newNode);
					}
				}
				if(square[2,1] && square[2,0])
				{
					IntVector2D newNode = new IntVector2D(node.x + 2, node.y - 1);
					if(CheckNode(newNode))
					{	
						list.Add(newNode);
					}
				}
				if(square[2,0] && square[1,0])
				{
					IntVector2D newNode = new IntVector2D(node.x + 1, node.y - 2);
					if(CheckNode(newNode))
					{	
						list.Add(newNode);
					}
				}
				if(square[0,0] && square[1,0])
				{
					IntVector2D newNode = new IntVector2D(node.x - 1, node.y - 2);
					if(CheckNode(newNode))
					{	
						list.Add(newNode);
					}
				}
				if(square[0,0] && square[0,1])
				{
					IntVector2D newNode = new IntVector2D(node.x - 2, node.y - 1);
					if(CheckNode(newNode))
					{	
						list.Add(newNode);
					}
				}
				if(square[0,2] && square[0,1])
				{
					IntVector2D newNode = new IntVector2D(node.x - 2, node.y + 1);
					if(CheckNode(newNode))
					{	
						list.Add(newNode);
					}
				}
				
				break;
			}
			}
			return list;
		}

		public bool CheckNode(IntVector2D node)
		{
			if(node.x < 0 || node.y < 0 || node.x >= gridSize || node.y >= gridSize) return false;
			return !tiles[node.x, node.y].blocked;
		}

		public List<IntVector2D> Search(IntVector2D start, IntVector2D end, int mode)
		{
			var comp = new IntVector2DEqualityComparer();
			var closedSet = new Dictionary<IntVector2D, GridNode>(comp);
			var map = new Dictionary<IntVector2D, GridNode>(comp);
			var openSet = new Dictionary<IntVector2D, GridNode>(comp);

			var startNode = new GridNode();
			startNode.f = IntVector2D.Distance(start, end);
			startNode.g = 0f;

			map[start] = startNode;
			openSet[start] = startNode;

			int tries = 0;

			while(openSet.Count > 0 && tries++ < maxTries)
			{

				bool setBestFirts = false;
				KeyValuePair<IntVector2D, GridNode> best = new KeyValuePair<IntVector2D, GridNode>();
				foreach(var pair in openSet)
				{
					if(!setBestFirts || best.Value.f > pair.Value.f)
					{
						best = pair;
						setBestFirts = true;
						continue;
					}
				}

				openSet.Remove(best.Key);
				closedSet[best.Key] = best.Value;

				if(comp.Equals(best.Key, end))
				{
					List<IntVector2D> path = new List<IntVector2D>();
					GridNode scan = best.Value;
					path.Add(end);
					while(scan != null && scan.cameFrom != null)
					{
						path.Insert(0, scan.cameFrom);
						scan = map[scan.cameFrom];
					}
					return path;
				}

				foreach(var cell in GetNeighbours(best.Key, mode))
				{
					if(closedSet.ContainsKey(cell))
					{
						continue;
					}

					float tentativeGScore = best.Value.g + IntVector2D.Distance(best.Key, cell);
					GridNode currentNode;

					if(!openSet.TryGetValue(cell, out currentNode))
				   	{
						currentNode = new GridNode();
						map[cell] = currentNode;
						openSet[cell] = currentNode;
					}

					if(currentNode.g > tentativeGScore)
					{
						currentNode.g = tentativeGScore;
						currentNode.cameFrom = best.Key;
						currentNode.f = tentativeGScore + IntVector2D.Distance(cell, end);
					}
				}
			}
			return null;

		}

		public Vector3 GridToWorldPosition(IntVector2D intPos)
		{
			return new Vector3((intPos.x + 0.5f - gridSize / 2) * tileSize, 0f
			                   , (intPos.y + 0.5f - gridSize / 2) * tileSize);
		}

		public List<Vector3> GridToWorldPosition(List<IntVector2D> intPoses)
		{
			List<Vector3> list = new List<Vector3>();
			foreach(IntVector2D intPos in intPoses)
			{

			list.Add( new Vector3((intPos.x + 0.5f - gridSize / 2) * tileSize, 0f
			                   , (intPos.y + 0.5f - gridSize / 2) * tileSize));
			}
			return list;
		}

	}

}