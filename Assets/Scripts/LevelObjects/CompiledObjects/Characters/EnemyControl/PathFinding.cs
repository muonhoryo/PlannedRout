

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using PlannedRout.LevelManagment;
using PlannedRout.LevelObjects.Characters;
using Unity.VisualScripting;
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
            List<Vector2Int> checkedCells = new List<Vector2Int> {Start };
            List<List<Vector2Int>> pathes= new List<List<Vector2Int>> { new List<Vector2Int> { Start } };

            Queue<(List<Vector2Int>,Vector2Int)> findQueue = new Queue<(List<Vector2Int>, Vector2Int)> {};
            findQueue.Enqueue((pathes[0],Start));


            List<Vector2Int> associatedPath;
            Vector2Int point;
            Vector2Int right,top,left,bottom;
            bool isAlreadyHasWay;

            while (findQueue.Count != 0)
            {
                (associatedPath, point) = findQueue.Dequeue();


                isAlreadyHasWay = false;

                right = new Vector2Int(point.x + 1, point.y);
                top = new Vector2Int(point.x, point.y + 1);
                left = new Vector2Int(point.x - 1,point.y);
                bottom = new Vector2Int(point.x, point.y - 1);

                bool CheckPoint(Vector2Int checkingPoint)
                {
                    if (LevelManager.Instance_.CheckCellPosition(checkingPoint.x, checkingPoint.y))
                    {
                        if (!checkedCells.Contains(checkingPoint))
                        {
                            checkedCells.Add(checkingPoint);
                            ILevelPart cell = LevelManager.Instance_.GetCell(checkingPoint.x, checkingPoint.y);
                            if (cell == null || cell.PartType_ != ILevelPart.LevelPartType.Wall)
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                }

                if(right==End||top==End||
                    left == End || bottom == End)
                {
                    Path = associatedPath;
                    Path.Add(End);
                    break;
                }
                else
                {

                    if (CheckPoint(right))
                    {
                        isAlreadyHasWay=true;
                        associatedPath.Add(right);
                        findQueue.Enqueue((associatedPath,right));
                    }

                    void CheckPoint_Next(Vector2Int checkedPoint)
                    {
                        if (CheckPoint(checkedPoint))
                        {
                            if (isAlreadyHasWay)
                                AddNewPath(checkedPoint);
                            else
                            {
                                isAlreadyHasWay = true;
                                associatedPath.Add(checkedPoint);
                                findQueue.Enqueue((associatedPath, checkedPoint));
                            }
                        }
                    }
                    void AddNewPath(Vector2Int newPathLastPoint)
                    {
                        var newPath = new List<Vector2Int>(associatedPath);
                        newPath[newPath.Count - 1] = newPathLastPoint;
                        pathes.Add(newPath);
                        findQueue.Enqueue((newPath, newPathLastPoint));
                    }

                    CheckPoint_Next(top);
                    CheckPoint_Next(left);
                    CheckPoint_Next(bottom);

                    if (!isAlreadyHasWay)
                        pathes.Remove(associatedPath);
                }
            }
        }
    }
}