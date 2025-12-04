using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

namespace Project.Scripts.GameLogic
{
    public class MapBuilder : MonoBehaviour
    {
        private static int Size = 5;
        
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private float tileSize;
        
        [SerializeField] private List<GameObject> Grid = new List<GameObject>();
        [SerializeField] private GameObject gridContainer;
        private NavMeshSurface surface;
        
        [SerializeField] private GameObject Player;

        private void Start()
        {
            surface = GetComponent<NavMeshSurface>();
            
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Grid.Add(Instantiate(tilePrefab, new Vector3(i * tileSize, 0, j * tileSize),
                        Quaternion.identity));
                    int index = UnityEngine.Random.Range(1, 4);
                    Grid[i * Size + j].transform.GetChild(index).gameObject.SetActive(true);
                    
                    Grid[i * Size + j].transform.SetParent(gridContainer.transform);
                    Grid[i * Size + j].transform.GetComponent<TileComponent>().IndexI = i;
                    Grid[i * Size + j].transform.GetComponent<TileComponent>().IndexJ = j;
                }
            }
            surface.BuildNavMesh();
            int pInd = (Size)/ 2;
            Player.transform.position = new Vector3(Grid[pInd * Size + pInd].transform.position.x, 
                Player.transform.position.y, Grid[pInd * Size + pInd].transform.position.z);
        }

        public void GridShift(int x, int z)
        {
            if (x < 0)
            {
                for (int i = Size - 1; i > Size - 1 + x; i--)
                {
                    for (int j = 0 ; j < Size; j++)
                        Destroy(Grid[i * Size + j]);
                }
            }
            else if (x > 0)
            {
                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < Size; j++)
                        Destroy(Grid[i * Size + j]);
                }
            }
            
            if (z < 0)
            {
                for (int j = Size - 1; j > Size - 1 + z; j--)
                {
                    for (int i = 0 ; i < Size; i++)
                        Destroy(Grid[i * Size + j]);
                }
            }
            else if (z > 0)
            {
                for (int j = 0; j < z; j++)
                {
                    for (int i = 0; i < Size; i++)
                        Destroy(Grid[i * Size + j]);
                }
            }

            if (x >= 0 && z >= 0)
            {
                for (int i = 0; i < Size; i++)
                {
                    if (i + x >= 0 && i + x <= Size - 1)
                    {
                        for (int j = 0; j < Size; j++)
                        {
                            if (j + z >= 0 && j + z <= Size - 1)
                            {
                                Grid[i * Size + j] = Grid[(i + x) * Size + j + z];
                            }
                            else
                            {
                                Grid[i * Size + j] = Instantiate(tilePrefab, new Vector3(
                                    Grid[i * Size + j].transform.position.x + x * tileSize, 0,
                                    Grid[i * Size + j].transform.position.z + z * tileSize), Quaternion.identity);
                                int index = UnityEngine.Random.Range(1, 4);
                                Grid[i * Size + j].transform.GetChild(index).gameObject.SetActive(true);
                                
                                Grid[i * Size + j].transform.SetParent(gridContainer.transform);
                            }

                            Grid[i * Size + j].transform.GetComponent<TileComponent>().IndexI = i;
                            Grid[i * Size + j].transform.GetComponent<TileComponent>().IndexJ = j;
                        }
                    }
                    else
                    {
                        for (int j = 0; j < Size; j++)
                        {
                            Grid[i * Size + j] = Instantiate(tilePrefab, new Vector3(
                                Grid[i * Size + j].transform.position.x + x * tileSize, 0,
                                Grid[i * Size + j].transform.position.z + z * tileSize), Quaternion.identity);
                            int index = UnityEngine.Random.Range(1, 4);
                            Grid[i * Size + j].transform.GetChild(index).gameObject.SetActive(true);
                            Grid[i * Size + j].transform.SetParent(gridContainer.transform);

                            Grid[i * Size + j].transform.GetComponent<TileComponent>().IndexI = i;
                            Grid[i * Size + j].transform.GetComponent<TileComponent>().IndexJ = j;
                        }
                    }
                }
            }
            else if (x >= 0 && z < 0)
            {
                for (int i = 0; i < Size; i++)
                {
                    if (i + x >= 0 && i + x <= Size - 1)
                    {
                        for (int j = Size - 1; j >= 0; j--)
                        {
                            if (j + z >= 0 && j + z <= Size - 1)
                            {
                                Grid[i * Size + j] = Grid[(i + x) * Size + j + z];
                            }
                            else
                            {
                                Grid[i * Size + j] = Instantiate(tilePrefab, new Vector3(
                                    Grid[i * Size + j].transform.position.x + x * tileSize, 0,
                                    Grid[i * Size + j].transform.position.z + z * tileSize), Quaternion.identity);
                                int index = UnityEngine.Random.Range(1, 4);
                                Grid[i * Size + j].transform.GetChild(index).gameObject.SetActive(true);
                                
                                Grid[i * Size + j].transform.SetParent(gridContainer.transform);
                            }

                            Grid[i * Size + j].transform.GetComponent<TileComponent>().IndexI = i;
                            Grid[i * Size + j].transform.GetComponent<TileComponent>().IndexJ = j;
                        }
                    }
                    else
                    {
                        for (int j = Size - 1; j >= 0; j--)
                        {
                            Grid[i * Size + j] = Instantiate(tilePrefab, new Vector3(
                                Grid[i * Size + j].transform.position.x + x * tileSize, 0,
                                Grid[i * Size + j].transform.position.z + z * tileSize), Quaternion.identity);
                            int index = UnityEngine.Random.Range(1, 4);
                            Grid[i * Size + j].transform.GetChild(index).gameObject.SetActive(true);
                            Grid[i * Size + j].transform.SetParent(gridContainer.transform);

                            Grid[i * Size + j].transform.GetComponent<TileComponent>().IndexI = i;
                            Grid[i * Size + j].transform.GetComponent<TileComponent>().IndexJ = j;
                        }
                    }
                }
            }
            else if (x < 0 && z >= 0)
            {
                for (int i = Size - 1; i >= 0; i--)
                {
                    if (i + x >= 0 && i + x <= Size - 1)
                    {
                        for (int j = 0; j < Size; j++)
                        {
                            if (j + z >= 0 && j + z <= Size - 1)
                            {
                                Grid[i * Size + j] = Grid[(i + x) * Size + j + z];
                            }
                            else
                            {
                                Grid[i * Size + j] = Instantiate(tilePrefab, new Vector3(
                                    Grid[i * Size + j].transform.position.x + x * tileSize, 0,
                                    Grid[i * Size + j].transform.position.z + z * tileSize), Quaternion.identity);
                                int index = UnityEngine.Random.Range(1, 4);
                                Grid[i * Size + j].transform.GetChild(index).gameObject.SetActive(true);
                                
                                Grid[i * Size + j].transform.SetParent(gridContainer.transform);
                            }

                            Grid[i * Size + j].transform.GetComponent<TileComponent>().IndexI = i;
                            Grid[i * Size + j].transform.GetComponent<TileComponent>().IndexJ = j;
                        }
                    }
                    else
                    {
                        for (int j = 0; j < Size; j++)
                        {
                            Grid[i * Size + j] = Instantiate(tilePrefab, new Vector3(
                                Grid[i * Size + j].transform.position.x + x * tileSize, 0,
                                Grid[i * Size + j].transform.position.z + z * tileSize), Quaternion.identity);
                            int index = UnityEngine.Random.Range(1, 4);
                            Grid[i * Size + j].transform.GetChild(index).gameObject.SetActive(true);
                            Grid[i * Size + j].transform.SetParent(gridContainer.transform);

                            Grid[i * Size + j].transform.GetComponent<TileComponent>().IndexI = i;
                            Grid[i * Size + j].transform.GetComponent<TileComponent>().IndexJ = j;
                        }
                    }
                }
            }
            else if (x < 0 && z < 0)
            {
                for (int i = Size - 1; i >= 0; i--)
                {
                    if (i + x >= 0 && i + x <= Size - 1)
                    {
                        for (int j = Size - 1; j >= 0; j--)
                        {
                            if (j + z >= 0 && j + z <= Size - 1)
                            {
                                Grid[i * Size + j] = Grid[(i + x) * Size + j + z];
                            }
                            else
                            {
                                Grid[i * Size + j] = Instantiate(tilePrefab, new Vector3(
                                    Grid[i * Size + j].transform.position.x + x * tileSize, 0,
                                    Grid[i * Size + j].transform.position.z + z * tileSize), Quaternion.identity);
                                int index = UnityEngine.Random.Range(1, 4);
                                Grid[i * Size + j].transform.GetChild(index).gameObject.SetActive(true);
                                
                                Grid[i * Size + j].transform.SetParent(gridContainer.transform);
                            }

                            Grid[i * Size + j].transform.GetComponent<TileComponent>().IndexI = i;
                            Grid[i * Size + j].transform.GetComponent<TileComponent>().IndexJ = j;
                        }
                    }
                    else
                    {
                        for (int j = Size - 1; j >= 0; j--)
                        {
                            Grid[i * Size + j] = Instantiate(tilePrefab, new Vector3(
                                Grid[i * Size + j].transform.position.x + x * tileSize, 0,
                                Grid[i * Size + j].transform.position.z + z * tileSize), Quaternion.identity);
                            int index = UnityEngine.Random.Range(1, 4);
                            Grid[i * Size + j].transform.GetChild(index).gameObject.SetActive(true);
                            Grid[i * Size + j].transform.SetParent(gridContainer.transform);
                            
                            Grid[i * Size + j].transform.GetComponent<TileComponent>().IndexI = i;
                            Grid[i * Size + j].transform.GetComponent<TileComponent>().IndexJ = j;
                        }
                    }
                }
            }
            
            surface.BuildNavMesh();
        }
    }
}