

using System.Collections.Generic;
using System.Linq;
using PlannedRout.LevelManagment;
using PlannedRout.LevelObjects.Characters;
using UnityEngine;

namespace PlannedRout
{
    public sealed class PathFinding
    {
        private PathFinding() { }
        public PathFinding(Vector2Int start,Vector2Int end)
        {
            if (!LevelManager.Instance_.CheckCellPosition(start.x, start.y))
                throw new System.Exception("Incorrect cell position.");
            if (!LevelManager.Instance_.CheckCellPosition(end.x, end.y))
                throw new System.Exception("Incorrect cell position.");

            Start = start;
            End = end;
        }

        private Vector2Int Start;
        private Vector2Int End;

        public List<Vector2Int> Path=null;

        public void FindPath()
        {
            List<Vector2Int> checkedCells= new List<Vector2Int>() { Start};
            List<Vector2Int> path = new List<Vector2Int> { Start };

            bool FindCycle(Vector2Int point) //return true if found path
            {
                Vector2Int newPos;
                bool CheckNextPoint()
                {
                    if (newPos == End)
                    {
                        path.Add(newPos);
                        return true;
                    }

                    if (checkedCells.Contains(newPos)||
                        !LevelManager.Instance_.CheckCellPosition(newPos.x,newPos.y))
                        return false;

                    checkedCells.Add(newPos);

                    if (IsPassableCell(LevelManager.Instance_.GetCell(newPos.x, newPos.y)))
                    {
                        path.Add(newPos);
                        bool isFound= FindCycle(newPos);
                        if(!isFound)
                            path.RemoveAt(path.Count - 1);
                        return isFound;
                    }
                    return false;
                }

                bool CheckRight()
                {
                    newPos = new Vector2Int(point.x + 1, point.y);
                    return CheckNextPoint();
                }
                bool CheckLeft()
                {
                    newPos = new Vector2Int(point.x - 1, point.y);
                    return CheckNextPoint();
                }
                bool CheckTop()
                {
                    newPos = new Vector2Int(point.x, point.y+1);
                    return CheckNextPoint();
                }
                bool CheckBottom()
                {
                    newPos = new Vector2Int(point.x, point.y-1);
                    return CheckNextPoint();
                }

                bool? isRightSide=null;
                bool? isTopSide=null;
                //Check priority directions
                if (point.x < End.x)
                {
                    isRightSide = true;
                    if (CheckRight())
                        return true;
                }
                else if(point.x >End.x)
                {
                    isRightSide = false;
                    if (CheckLeft())
                        return true;
                }

                if (point.y < End.y)
                {
                    isTopSide = true;
                    if (CheckTop())
                        return true;
                }
                else if (point.y > End.y)
                {
                    isTopSide = false;
                    if (CheckBottom())
                        return true;
                }
                //Check other directions
                if (isRightSide == null || (bool)isRightSide)
                {
                    if (CheckLeft())
                        return true;
                }
                else
                {
                    if (CheckRight())
                        return true;
                }

                if(isTopSide==null||(bool)isTopSide)
                {
                    if (CheckBottom())
                        return true;
                }
                else
                {
                    if (CheckTop())
                        return true;
                }

                return false;
            }
            bool IsPassableCell(ILevelPart cell) =>
                cell==null ||cell.PartType_ != ILevelPart.LevelPartType.Wall;

            if (FindCycle(Start))
                Path = path;
        }
    }
}