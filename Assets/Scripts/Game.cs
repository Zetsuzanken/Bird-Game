using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {

    public static Game Instance;

    public GameObject TreePrefab;
    public float TreeDistance = 1f;
    public float Speed = 1f;
	
    private List<GameObject> _trees;

    private bool _stop;

	void Start () {
        _stop = false;
        Instance = this;
        _trees = new List<GameObject>();
        for (int i = 0; i < 3; i++)
        {
            GameObject tree = GameObject.Instantiate(TreePrefab);
            _trees.Add(tree);
            tree.transform.position = new Vector3(TreeDistance * i, Random.Range(-2f, 2f), 0f);
        }
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (!_stop)
        {
            foreach (GameObject tree in _trees)
            {
                tree.transform.position -= new Vector3(Time.deltaTime * Speed, 0f, 0f);
                if (tree.transform.position.x < -TreeDistance * (_trees.Count / 2f))
                {
                    tree.transform.position += new Vector3(TreeDistance * _trees.Count, 0f, 0f);
                }
            }
        }
	}

    public void Restart()
    {
        foreach (GameObject tree in _trees)
        {
            GameObject.Destroy(tree);
        }
        Start();
    }

    public void StopGame()
    {
        _stop = true;
    }
}
