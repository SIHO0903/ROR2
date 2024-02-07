using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

interface IInteractable
{
    public void Interact(int gold);
    public void SetActiveFalse();
}
interface IIBoss
{
    public void SpawnBoss();
}

public class Interactable : MonoBehaviour
{
    [Header("Interactable")]
    [SerializeField] TextMeshProUGUI interactionTxt;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] float range;
    [SerializeField] float boxCastSize;
    int itemIndex;
    Item item;
    private void Awake()
    {
        item = GetComponentInChildren<Item>();
    }
    public void InteractableObj()
    {
        Transform cameraTr = Camera.main.transform;
        if (Physics.BoxCast(cameraTr.position, transform.lossyScale * boxCastSize, cameraTr.forward, out RaycastHit hitInfo, Quaternion.identity, range, interactableLayer))
        {

            interactionTxt.gameObject.SetActive(true);
            if(LayerMask.LayerToName(hitInfo.transform.gameObject.layer) == "Box")
            {
                
                if (hitInfo.transform.name == "GoldChest")
                {
                    GameManager.instance.neededGold = 25 * 7;
                    interactionTxt.text = "E 상호작용 (" + GameManager.instance.neededGold + "$)";
                }
                else
                {
                    GameManager.instance.neededGold = 25;
                    interactionTxt.text = "E 상호작용 (" + GameManager.instance.neededGold + "$)";
                }

                IInteractable interactable = hitInfo.transform.GetComponent<IInteractable>();
                interactable.Interact(GameManager.instance.neededGold);
                interactable.SetActiveFalse();
                if (Input.GetKeyDown(KeyCode.E) && GameManager.instance.gold >= GameManager.instance.neededGold)
                {
                    hitInfo.transform.GetComponent<Animator>().SetTrigger("Open");
                    hitInfo.transform.GetComponent<BoxCollider>().enabled = false;
                    GameManager.instance.gold -= GameManager.instance.neededGold;
                    Destroy(hitInfo.transform.gameObject, 5f); //파괴??

                    StartCoroutine(ItemInstantiate(hitInfo.transform));

                }
                else if (Input.GetKeyDown(KeyCode.E) && GameManager.instance.gold < GameManager.instance.neededGold)
                {
                    Debug.Log("그지는가라");
                }


            }
            else if(LayerMask.LayerToName(hitInfo.transform.gameObject.layer) == "Teleporter")
            {
                interactionTxt.text = "E 상호작용 (보스 소환)";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (hitInfo.transform.parent.parent.gameObject.TryGetComponent(out IIBoss spawnManager))
                    {
                        spawnManager.SpawnBoss();
                    }
                    //hitInfo.transform.gameObject.GetComponentInParent<SpawnManager>().SpawnBoss();
                }
            }
        }
        else
        {
            interactionTxt.gameObject.SetActive(false);
        }

    }
    IEnumerator ItemInstantiate(Transform hitInfo)
    {
        yield return new WaitForSeconds(1f);
        GameObject itemTest = PoolManager.instance.Get(PoolManager.PrefabType.Items, Random.Range(0, PoolManager.instance.prefabDatas[4].pools.Length)); //랜텀아이템나오게
        itemTest.transform.position = hitInfo.position;
        itemTest.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            item.ItemMeshOn(other.gameObject.name,other.GetComponent<SpriteRenderer>().sprite);
            other.gameObject.SetActive(false);
        }
    }
}
