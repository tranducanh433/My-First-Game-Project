using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindingPath : MonoBehaviour
{
    public bool displayGizmos = true;

    public enum TargetHorizonDirection
    {
        Left, Right
    }
    public enum TargetVerticalDirection
    {
        Up, Down
    }
    Vector2 m_current;
    Vector2 m_target;
    LayerMask m_wallLayer;
    TargetHorizonDirection targetHorizonDirection;
    TargetVerticalDirection targetVerticalDirection;
    int distantCheck = 15;

    Vector2[] paths;

    public Vector2[] GetPathToTarget(Vector2 current, Vector2 target, LayerMask wallLayer)
    {
        m_current = current;
        m_target = target;
        m_wallLayer = wallLayer;

        // Where is target
        if(target.x < current.x)
        {
            targetHorizonDirection = TargetHorizonDirection.Left;
        }
        else //if (target.x > current.x)
        {
            targetHorizonDirection = TargetHorizonDirection.Right;
        }

        if (target.y < current.y)
        {
            targetVerticalDirection = TargetVerticalDirection.Down;
        }
        else //if (target.y > current.y)
        {
            targetVerticalDirection = TargetVerticalDirection.Up;
        }

        // FindPath
        Vector2[] leftPath = FindTargetFromLeft(current);
        Vector2[] topPath = FindTargetFromTop(current);
        Vector2[] rightPath = FindTargetFromRight(current);
        Vector2[] downPath = FindTargetFromDown(current);

        paths = GetTheShortestPath(leftPath, topPath, rightPath, downPath);
        return paths;
    }

    private Vector2[] GetTheShortestPath(Vector2[] leftPath, Vector2[] topPath, Vector2[] rightPath, Vector2[] downPath)
    {
        List<float> distancePaths = new List<float>();
        distancePaths.Add(ToDistance(leftPath));
        distancePaths.Add(ToDistance(topPath));
        distancePaths.Add(ToDistance(rightPath));
        distancePaths.Add(ToDistance(downPath));

        List<Vector2[]> paths = new List<Vector2[]>();
        paths.Add(leftPath);
        paths.Add(topPath);
        paths.Add(rightPath);
        paths.Add(downPath);
        int shortestPathIndex = -1;


        for (int i = 0; i < distancePaths.Count; i++)
        {
            if (distancePaths[i] != 0 && shortestPathIndex == -1)
            {
                shortestPathIndex = i;
            }

            else if (shortestPathIndex != -1)
            {
                if(distancePaths[shortestPathIndex] > distancePaths[i] && distancePaths[i] != 0)
                {
                    shortestPathIndex = i;
                }
            }
        }

        if (shortestPathIndex == -1)
            return null;
        else
            return paths[shortestPathIndex];
    }

    private float ToDistance(Vector2[] path)
    {
        if (path.Length == 0) return 0;

        float distence = Vector2.Distance(m_current, path[0]);

        for (int i = 1; i < path.Length; i++)
        {
            distence += Vector2.Distance(path[i - 1], path[i]);
        }
        distence += Vector2.Distance(path[path.Length - 1], m_target);

        return distence;
    }


     Vector2[] FindTargetFromLeft(Vector2 startPoint, bool oneStep = false)
    {
        List<Vector2> path = new List<Vector2>();
        bool complete = false;

        // Plan 1
        for (int i = 0; i < 20; i++)
        {
            Vector2 pointCheck = new Vector2(startPoint.x - i, startPoint.y);

            if (Physics2D.Linecast(startPoint, pointCheck, m_wallLayer) == false)
            {
                if(Physics2D.Linecast(pointCheck, m_target, m_wallLayer) == false)
                {
                    complete = true;
                    path.Add(pointCheck);
                    break;
                }
            }
            else
            {
                break;
            }
        }

        if (complete == false && oneStep == false)
        {
            // Plan 2 (if plan 1 fall)

            if (targetVerticalDirection == TargetVerticalDirection.Up)
            {
                for (int x = 0; x < distantCheck; x++)
                {
                    Vector2 pointCheck1 = new Vector2(startPoint.x - x, startPoint.y);

                    if (Physics2D.Linecast(startPoint, pointCheck1, m_wallLayer) == false)
                    {
                        Vector2[] pointCheck2 = FindTargetFromTop(pointCheck1, true);

                        if (pointCheck2 == null)
                            continue;
                        else if (pointCheck2.Length == 0)
                            continue;

                        path.Add(pointCheck1);
                        path.Add(pointCheck2[0]);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else if (targetVerticalDirection == TargetVerticalDirection.Down)
            {
                for (int x = 0; x < distantCheck; x++)
                {
                    Vector2 pointCheck1 = new Vector2(startPoint.x - x, startPoint.y);

                    if (Physics2D.Linecast(startPoint, pointCheck1, m_wallLayer) == false)
                    {
                        Vector2[] pointCheck2 = FindTargetFromDown(pointCheck1, true);

                        if (pointCheck2 == null)
                            continue;
                        else if (pointCheck2.Length == 0)
                            continue;

                        path.Add(pointCheck1);
                        path.Add(pointCheck2[0]);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        return path.ToArray();
    }

     Vector2[] FindTargetFromTop(Vector2 startPoint, bool oneStep = false)
    {
        List<Vector2> path = new List<Vector2>();
        bool complete = false;

        // Plan 1
        for (int i = 0; i < distantCheck; i++)
        {
            Vector2 pointCheck = new Vector2(startPoint.x, startPoint.y + i);

            if (Physics2D.Linecast(startPoint, pointCheck, m_wallLayer) == false)
            {
                if (Physics2D.Linecast(pointCheck, m_target, m_wallLayer) == false)
                {
                    complete = true;
                    path.Add(pointCheck);
                    break;
                }
            }
            else
            {
                break;
            }
        }

        if (complete == false && oneStep == false)
        {
            // Plan 2 (if plan 1 fall)

            if (targetHorizonDirection == TargetHorizonDirection.Left)
            {
                for (int y = 0; y < distantCheck; y++)
                {
                    Vector2 pointCheck1 = new Vector2(startPoint.x, startPoint.y + y);

                    if (Physics2D.Linecast(startPoint, pointCheck1, m_wallLayer) == false)
                    {
                        Vector2[] pointCheck2 = FindTargetFromLeft(pointCheck1, true);

                        if (pointCheck2 == null)
                            continue;
                        else if (pointCheck2.Length == 0)
                            continue;

                        path.Add(pointCheck1);
                        path.Add(pointCheck2[0]);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else if (targetHorizonDirection == TargetHorizonDirection.Right)
            {
                for (int y = 0; y < distantCheck; y++)
                {
                    Vector2 pointCheck1 = new Vector2(startPoint.x, startPoint.y + y);

                    if (Physics2D.Linecast(startPoint, pointCheck1, m_wallLayer) == false)
                    {
                        Vector2[] pointCheck2 = FindTargetFromRight(pointCheck1, true);

                        if (pointCheck2 == null)
                            continue;
                        else if (pointCheck2.Length == 0)
                            continue;

                        path.Add(pointCheck1);
                        path.Add(pointCheck2[0]);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        return path.ToArray();
    }

     Vector2[] FindTargetFromRight(Vector2 startPoint, bool oneStep = false)
    {
        List<Vector2> path = new List<Vector2>();
        bool complete = false;

        // Plan 1
        for (int i = 0; i < distantCheck; i++)
        {
            Vector2 pointCheck = new Vector2(startPoint.x + i, startPoint.y);

            if (Physics2D.Linecast(startPoint, pointCheck, m_wallLayer) == false)
            {
                if (Physics2D.Linecast(pointCheck, m_target, m_wallLayer) == false)
                {
                    complete = true;
                    path.Add(pointCheck);
                    break;
                }
            }
            else
            {
                break;
            }
        }

        if (complete == false && oneStep == false)
        {
            // Plan 2 (if plan 1 fall)

            if (targetVerticalDirection == TargetVerticalDirection.Up)
            {
                for (int x = 0; x < distantCheck; x++)
                {
                    Vector2 pointCheck1 = new Vector2(startPoint.x + x, startPoint.y);

                    if (Physics2D.Linecast(startPoint, pointCheck1, m_wallLayer) == false)
                    {
                        Vector2[] pointCheck2 = FindTargetFromTop(pointCheck1, true);

                        if (pointCheck2 == null)
                            continue;
                        else if (pointCheck2.Length == 0)
                            continue;

                        path.Add(pointCheck1);
                        path.Add(pointCheck2[0]);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else if (targetVerticalDirection == TargetVerticalDirection.Down)
            {
                for (int x = 0; x < distantCheck; x++)
                {
                    Vector2 pointCheck1 = new Vector2(startPoint.x + x, startPoint.y);

                    if (Physics2D.Linecast(startPoint, pointCheck1, m_wallLayer) == false)
                    {
                        Vector2[] pointCheck2 = FindTargetFromDown(pointCheck1, true);

                        if (pointCheck2.Length == 0)
                            continue;
                        else if (pointCheck2.Length == 0)
                            continue;

                        path.Add(pointCheck1);
                        path.Add(pointCheck2[0]);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        return path.ToArray();
    }

     Vector2[] FindTargetFromDown(Vector2 startPoint, bool oneStep = false)
    {
        List<Vector2> path = new List<Vector2>();
        bool complete = false;

        // Plan 1
        for (int i = 0; i < distantCheck; i++)
        {
            Vector2 pointCheck = new Vector2(startPoint.x, startPoint.y - i);

            if (Physics2D.Linecast(startPoint, pointCheck, m_wallLayer) == false)
            {
                if (Physics2D.Linecast(pointCheck, m_target, m_wallLayer) == false)
                {
                    complete = true;
                    path.Add(pointCheck);
                    break;
                }
            }
            else
            {
                break;
            }
        }

        if (complete == false && oneStep == false)
        {
            // Plan 2 (if plan 1 fall)

            if (targetHorizonDirection == TargetHorizonDirection.Left)
            {
                for (int y = 0; y < distantCheck; y++)
                {
                    Vector2 pointCheck1 = new Vector2(startPoint.x, startPoint.y - y);

                    if (Physics2D.Linecast(startPoint, pointCheck1, m_wallLayer) == false)
                    {
                        Vector2[] pointCheck2 = FindTargetFromLeft(pointCheck1, true);

                        if (pointCheck2 == null)
                            continue;
                        else if (pointCheck2.Length == 0)
                            continue;

                        path.Add(pointCheck1);
                        path.Add(pointCheck2[0]);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else if (targetHorizonDirection == TargetHorizonDirection.Right)
            {
                for (int y = 0; y < distantCheck; y++)
                {
                    Vector2 pointCheck1 = new Vector2(startPoint.x, startPoint.y - y);

                    if (Physics2D.Linecast(startPoint, pointCheck1, m_wallLayer) == false)
                    {
                        Vector2[] pointCheck2 = FindTargetFromRight(pointCheck1, true);

                        if (pointCheck2 == null)
                            continue;
                        else if (pointCheck2.Length == 0)
                            continue;

                        path.Add(pointCheck1);
                        path.Add(pointCheck2[0]);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        return path.ToArray();
    }

    private void OnDrawGizmos()
    {
        if (paths == null || displayGizmos == false)
            return;
        if (paths.Length == 0)
            return;

        Gizmos.DrawLine(m_current, paths[0]);
        for (int i = 1; i < paths.Length; i++)
        {
            Gizmos.DrawLine(paths[i - 1], paths[i]);
        }
        Gizmos.DrawLine(paths[paths.Length - 1], m_target);
    }
}
