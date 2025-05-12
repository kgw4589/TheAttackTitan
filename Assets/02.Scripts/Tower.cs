using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public static Tower Instance;
    
    public Transform damageUI;
    public Image damageImage;

    private float _damageTime = 0.1f;

    private const int InitialHp = 10;
    private int _currentHp;

    public Text towerHpText;

    public int CurrentHp
    {
        get { return _currentHp; }
        set
        {
            _currentHp = value;

            if (_currentHp < 0)
            {
                _currentHp = 0;
            }
            
            towerHpText.text = $"타워 체력 : {CurrentHp}";
            
            StopAllCoroutines();
            StartCoroutine(DamageAction());
            
            if (_currentHp <= 0)
            {
                Destroy(gameObject);
                GameManager.Instance.LoseGame();
            }
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    private void Start()
    {
        Camera camera = Camera.main;
        
        _currentHp = InitialHp;
        towerHpText.text = $"타워 체력 : {CurrentHp}";
        
        float z = camera.nearClipPlane + 0.01f;

        damageUI.parent = camera.transform;
        damageUI.localPosition = new Vector3(0, 0, z);
        damageImage.enabled = false;
    }

    private IEnumerator DamageAction()
    {
        damageImage.enabled = false;
        damageImage.enabled = true;
        
        yield return new WaitForSeconds(_damageTime);

        damageImage.enabled = false;
    }
}
