using MyExtension;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItemEvent : MonoBehaviour
{
    public static GetItemEvent instance;

    public GameObject getItemEventPrefab;

    List<Vector2> eventPos = new List<Vector2>();
    List<ItemGetContent> itemContent = new List<ItemGetContent>();
    List<GameObject> eventObj = new List<GameObject>();
    List<float> availableTime = new List<float>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        for (int i = 0; i < availableTime.Count; i++)
        {
            availableTime[i] -= Time.deltaTime;

            if(availableTime[i] <= 1)
            {
                eventPos[i] = new Vector2(transform.position.x, eventPos[i].y);
                itemContent[i].SetPosition(eventPos[i]);
            }
            if(availableTime[i] <= 0)
            {
                RemoveEvent(i);
            }
        }
    }

    public void NewItemEvent(ItemBase item)
    {
        if(eventPos.Count == 0)
        {
            CreateNew(item);
        }
        else
        {
            int element = IsItemContentHaveYet(item);

            if (element != -1)
            {
                AddValue(element);
            }
            else
            {
                for (int i = 0; i < eventPos.Count; i++)
                {
                    eventPos[i] = new Vector2(eventPos[i].x, eventPos[i].y + 100);
                    itemContent[i].SetPosition(eventPos[i]);
                }

                CreateNew(item);
            }
        }
    }
    int IsItemContentHaveYet(ItemBase item)
    {
        for (int i = 0; i < itemContent.Count; i++)
        {
            if (itemContent[i].currentItem == item && availableTime[i] > 1)
            {
                return i;
            }
        }

        return -1;
    }

    void AddValue(int element)
    {
        itemContent[element].AddAmount();
        availableTime[element] = 4;
    }
    
    void CreateNew(ItemBase item)
    {
        Vector2 position = new Vector2(transform.position.x + 500, transform.position.y);

        GameObject obj_event = Instantiate(getItemEventPrefab, position, Quaternion.identity, transform);
        ItemGetContent itemGetContent = obj_event.GetComponent<ItemGetContent>();
        itemGetContent.SetItem(item, position, 5f);

        eventObj.Add(obj_event);
        eventPos.Add(position);
        availableTime.Add(4);
        itemContent.Add(itemGetContent);
    }

    void RemoveEvent(int element)
    {
        for (int i = element; i >= 0; i--)
        {
            eventPos[i] = new Vector2(eventPos[i].x, eventPos[i].y - 100);
            itemContent[i].SetPosition(eventPos[i]);
        }

        eventPos.RemoveAt(element);
        availableTime.RemoveAt(element);
        itemContent.RemoveAt(element);

        Destroy(eventObj[element]);
        eventObj.RemoveAt(element);
    }
}
