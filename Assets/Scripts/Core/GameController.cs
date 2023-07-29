using Task.Helper;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private int redValue = 10;
    private int pinkValue = 10;
    private int greenValue = 10;
    [SerializeField] private TextMeshProUGUI greenText;
    [SerializeField] private TextMeshProUGUI pinkText;
    [SerializeField] private TextMeshProUGUI redText;
    [SerializeField] private WorldManager worldManager;
    public void ChangeValue(ObjectType type, int value)
    {
        switch (type)
        {
            case ObjectType.Red:
                redValue += value;
                redText.text = redValue.ToString();
                break;
            case ObjectType.Green:
                greenValue += value;
                greenText.text = greenValue.ToString();
                break;
            case ObjectType.Pink:
                pinkValue += value;
                pinkText.text = pinkValue.ToString();
                break;
        }

    }
    private bool CheckIfBlockExits(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.Red:
                return redValue > 0;

            case ObjectType.Green:
                return greenValue > 0;

            case ObjectType.Pink:
                return pinkValue > 0;

        }
        return false;
    }

    /// <summary>
    /// This Function removes the midpointround depending on the even number.
    /// </summary>
    /// <param name="initialFloat"></param>
    /// <returns></returns>
    private int RoundToInt(float initialFloat)
    {
        int returnValue = Mathf.FloorToInt(initialFloat);
     
        int integerInHundred = returnValue * 100;
        int conversionInHundred = (int)(initialFloat * 100);
        if ((conversionInHundred - integerInHundred) <= 50)
        {
            return returnValue;
        }
        return returnValue + 1;
    }
   
    private int RoundToIntOtherWay(float initialFloat)
    {        
        int returnValue = Mathf.FloorToInt(initialFloat);
        int integerInHundred = returnValue * 100;
        int conversionInHundred = (int)(initialFloat * 100);
        if (conversionInHundred - integerInHundred <= 50)
        {
            return returnValue + 1;
        }
        return returnValue ;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 hitPoint = hit.point;
                int x = hitPoint.x - Mathf.Floor(hitPoint.x) == 0.5f && mousePoint.x - hitPoint.x < 0 ? RoundToIntOtherWay(hit.point.x) : RoundToInt(hit.point.x) ;
                int y = hitPoint.y - Mathf.Floor(hitPoint.y) == 0.5f && mousePoint.y - hitPoint.y < 0 ? RoundToIntOtherWay(hit.point.y) : RoundToInt(hit.point.y) ;
                int z = hitPoint.z - Mathf.Floor(hitPoint.z) == 0.5f && mousePoint.z - hitPoint.z < 0 ? RoundToIntOtherWay(hit.point.z) : RoundToInt(hit.point.z) ;
                string key = Helpers.GenerateGenericKey(x, y, z);
               Item item = worldManager.RemoveBlock(key);
               
                if (item != null)
                    ChangeValue(item.ObjectType, 1);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            worldManager.CaptureState();
        }
    }

    public void AddBlockToGame(Vector3 point, ObjectType objectType)
    {
        if (!CheckIfBlockExits(objectType)) return;
        Ray ray = Camera.main.ScreenPointToRay(point);
      
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(point);
            Vector3 hitPoint = hit.point;      
            int x = hitPoint.x - Mathf.Floor(hitPoint.x) == 0.5f ? (mousePoint.x - hitPoint.x < 0 ? 0 : 1) : 0;
            int y = hitPoint.y - Mathf.Floor(hitPoint.y) == 0.5f ? (mousePoint.y - hitPoint.y < 0 ? 0 : 1) : 0;
            int z = hitPoint.z - Mathf.Floor(hitPoint.z) == 0.5f ? (mousePoint.z - hitPoint.z < 0 ? 0 : 1) : 0;
            bool isFeasible = worldManager.AddBlockOnPoint(RoundToInt(hitPoint.x) + x, RoundToInt(hitPoint.y) + y, RoundToInt(hitPoint.z) + z,(int)objectType );
            if(isFeasible)
                ChangeValue(objectType, -1);
        }
    }

   
}
