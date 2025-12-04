using UnityEngine;

namespace Project.Scripts.PlayerLogic.Drawing
{
    public class LineGeneration : MonoBehaviour
    {
        [SerializeField] private GameObject linePrefab;
        [SerializeField] private GameObject currentLine;
        private Line activeLine;

        [SerializeField] private Transform canvas;
        private Camera cam;

        private void Start()
        {
            cam = Camera.main;
        }
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (currentLine != null)
                    Destroy(currentLine);
                currentLine = Instantiate(linePrefab);
                activeLine = currentLine.GetComponent<Line>();
                currentLine.transform.SetParent(canvas.transform);
            }
            if (Input.GetMouseButtonUp(0))
            {
                activeLine = null;
            }

            if (activeLine != null)
            {
                Vector2 mousePosition = Input.mousePosition;
                activeLine.UpdateLine(mousePosition);
            }
        }
    }
}