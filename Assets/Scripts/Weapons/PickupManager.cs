using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{

    public Transform cameraTransform;
    public KeyCode pickupKey = KeyCode.F;
    public KeyCode dropKey = KeyCode.G;
    string weaponTag = "Weapons";

    public List<GameObject> Weapon;
    public int maxWeapons = 2;

    // this variable represent the weapon you carry in your hand 
    public GameObject currentWeapon;

    // this variable represent your hand which you set as the parent of your currentWeapon
    public Transform hand;

    private Camera cam;

    // Insert a gameobject which you drop inside your player gameobject and position it where you want to drop items from
    // to avoid dropping items inside your player
    public Transform dropPoint;


    void Start()
    {

        cam = GameObject.Find("MainCamera").GetComponent<Camera>();
        cameraTransform = cam.transform;

        foreach (Transform child in hand)
        {
            Weapon.Add(child.gameObject);
            child.GetComponent<GunSystem>().enb = true;
        }

        SelectWeapon(0);
        
    }

    void Update()
    {
       

        // SELECT WEAPONS
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectWeapon(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectWeapon(1);
        }

        // PICKUP WEAPONS
        RaycastHit hit;
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        if (Physics.Raycast(ray, out hit))
        {

            if (hit.transform.CompareTag(weaponTag) && Input.GetKeyDown(pickupKey) && Weapon.Count < maxWeapons)
            {

                // save the weapon                
                Weapon.Add(hit.collider.gameObject);

                // hides the weapon because it's now in our 'inventory'
                hit.collider.gameObject.SetActive(false);

                // now we can positioning the weapon at many other places.
                // but for this demonstration where we just want to show a weapon
                // in our hand at some point we do it now.
                hit.transform.parent = hand;
                hit.transform.localPosition = new Vector3(0.337f, -0.36f, 0.63f);
                

                if(hit.collider.gameObject.name == "Shotgun")
                {
                    hit.transform.localRotation = Quaternion.Euler(0, 270.0f, 0);
                }
                else
                {
                    hit.transform.localRotation = new Quaternion(0, 0, 0, 0);
                }

                foreach (Transform child in hand)
                {
                    child.GetComponent<GunSystem>().enb = true;
                }
            }
        }

        // DROP WEAPONS
        // So let's say you wanted to add a feature where you wanted to drop the weapon you carry in your hand
        if (Input.GetKeyDown(dropKey) && currentWeapon != null)
        {

            // First ensure we remove our hand as parent for the weapon
            currentWeapon.transform.parent = null;

            // Move the weapon to the drop position
            currentWeapon.transform.position = dropPoint.position;

            // Remove it from our 'inventory'            
            var weaponInstanceId = currentWeapon.GetInstanceID();

            
            for (int i = 0; i < Weapon.Count; i++)
            {
                
                if ( Weapon[i].GetInstanceID() == weaponInstanceId)
                {
                    Weapon[i].GetComponent<GunSystem>().enb = false;
                    
                    Weapon.RemoveAt(i);

                    Weapon.Clear();

                    break;
                }
            }

            // Remove it from our 'hand'
            currentWeapon = null;
            Start();


        }
    }

    void SelectWeapon(int index)
    {

       // Debug.Log("Weapon count: " + Weapon.Count);
        // Ensure we have a weapon in the wanted 'slot'
        if(Weapon.Count ==2)
        {
            if (Weapon.Count > index && Weapon[index] != null)
            {

                // Check if we already is carrying a weapon
                if (currentWeapon != null)
                {
                    // hide the weapon                
                    currentWeapon.gameObject.SetActive(false);
                    if (index == 0)
                    {
                        currentWeapon = Weapon[0];
                    }
                    else if (index == 1)
                    {
                        currentWeapon = Weapon[1];
                    }
                }

                currentWeapon.gameObject.SetActive(true);
            }
        }
        else
        {
            if (Weapon.Count > index && Weapon[index] != null)
            {
                currentWeapon = Weapon[0];
                currentWeapon.gameObject.SetActive(true);

                
            }
        }

        Weapon[index].GetComponent<GunSystem>().text.SetText(Weapon[index].GetComponent<GunSystem>().bulletsLeft + " / " + Weapon[index].GetComponent<GunSystem>().magazineSize);


    }


}