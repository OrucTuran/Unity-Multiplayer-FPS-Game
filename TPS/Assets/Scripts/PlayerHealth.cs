using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerHealth : NetworkBehaviour
{
    [SyncVar]
    public int CurrentHealth=100;
    [SyncVar] public bool isDead = false;

    [SyncVar] private int kills = 0; 
    [SyncVar] private int deaths =  0; 
    Vector3 SpawnPoint;
    [SerializeField] private GameObject[] diasbleOnDeath;
   

    public void Damage(int DamageAmount)
    {
        if (CurrentHealth >= 0)
        {
            print("Damage");
            CurrentHealth -= DamageAmount;
        }
        

        if (CurrentHealth<=0 && isDead == false)
        {
            Die();
        }
    }
    private void Die()
    {
        isDead = true;
        IncreaseDeathCounter();
        HideGameObjectsUponDeath();
        StartCoroutine(Respawn());
    }

    [Server]
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3f); //amount of time before repawn
        int spawnPointplace = Random.Range(1, 4);
        Debug.Log(spawnPointplace.ToString());
        if (spawnPointplace == 1)
        {
            SpawnPoint = new Vector3(10f, -6f, -12f); // were we wanna spawn
        }else if (spawnPointplace == 2)
        {
            SpawnPoint = new Vector3(10f, -6f, 12f); // were we wanna spawn
        }
        else if (spawnPointplace == 3)
        {
           SpawnPoint = new Vector3(-27f, -6f, 19.5f); // were we wanna spawn
        }
        else if (spawnPointplace == 3)
        {
           SpawnPoint = new Vector3(-27f, -6f, -15.5f); // were we wanna spawn
        }

        transform.position = SpawnPoint;
        transform.rotation = Quaternion.identity;

        yield return new WaitForSeconds(0.1f); // respawning

        ShowGameObjectsUponRespawn();
        SetDefaultValues();
        //print("Test");
        //NetworkServer.UnSpawn(go);
        //Transform newPos = NetworkManager.singleton.GetStartPosition();
        //go.transform.position = newPos.position;
        //go.transform.rotation = newPos.rotation;
        //yield return new WaitForSeconds(0.5f);
        //NetworkServer.Spawn(go, go);
    }

    private void SetDefaultValues()
    {
        CurrentHealth = 100;
        isDead = false;
    }
    private void IncreaseDeathCounter()
    {
        deaths++;
    }

    private void HideGameObjectsUponDeath()
    {
        foreach(var item in diasbleOnDeath)
        {
            item.SetActive(false); //puts all items on the player to false;
        }
    }

    private void ShowGameObjectsUponRespawn()
    {
        foreach (var item in diasbleOnDeath)
        {
            item.SetActive(true); //puts all items on the player to false;
        }
    }

    public bool IsDead()
    {
        return isDead;
    }
    //private void SpectateWorldWhileDead()
    //{
    //    if (!isLocalPlayer)
    //    {
    //        return;
    //    }
    //    var PlayerSpectateMountPoint = GameObject.FindGameObjectsWithTag("SpectatePoint").ToList().First;
    //    var PlayerCameraTransform = PlayerCamera.GetCamera().gameObject.transform;
    //}

}
