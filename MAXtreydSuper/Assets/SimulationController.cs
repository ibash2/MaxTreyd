using DG.Tweening; 
using TMPro; 
using UnityEngine;  

public class SimulationController : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] TextMeshProUGUI _text2;



    [SerializeField] Transform _robot;
    [SerializeField] Transform _robot2;
    [SerializeField] Transform _startPoint;
    [SerializeField] Transform _finishPoint;
    [SerializeField] GameObject _item;

    [SerializeField] bool _isOn;
    [SerializeField] bool _isPaint;
    private GameObject newGameObject;

    public bool CanSpawnNew;
    public bool IsOn
    {
        get => _isOn;
        set
        {
            _isOn = value;
            if (value)
            {
                _text2.text = "Конвейер запущен!";
                _robot.DORotate(new Vector3(-90, 0, -90), 1);
                _robot2.DORotate(new Vector3(-90, 0, 90), 1);
                MoveNewItem();
            }
        }
    }

    public bool IsPaint
    {
        get => _isPaint; set
        {

            _text2.text = "Начинаем покраску!";
            _isPaint = value;
            if (_isPaint)
            {
                CanSpawnNew = false;
                _robot.DORotate(new Vector3(-90, 0, -270), 1).OnComplete(DoPaint);
                _robot2.DORotate(new Vector3(-90, 0, 270), 1).OnComplete(DoPaint);
            }
            else
            {
                _robot.DORotate(new Vector3(-90, 0, -90), 1);
                _robot2.DORotate(new Vector3(-90, 0, 90), 1);
            }
        }
    }

    public int Counter { get; private set; }

    public void SwitchMovePlatformState()
    {
        if (CanSpawnNew)
            IsOn = !IsOn;
    }
    public void SwitchPaintMode()
    {
        IsPaint = true; 
    }
    // Start is called before the first frame update
    void Start()
    { 
        Counter = 0;
        _text2.text = "Запустите конвейер!";
        newGameObject = Instantiate(_item);
        IsOn = false;
        IsPaint = false;
        CanSpawnNew = true;
    }

    // Update is called once per frame
    public void MoveNewItem()
    {
        Counter++;
        newGameObject.transform.position = _startPoint.position;
        newGameObject.transform.DOMove(_finishPoint.position, 5);
        _text.text = "Обработано моделей : " + Counter.ToString();
    }

    public void DoPaint()
    {
        newGameObject.GetComponent<Renderer>().material.DOColor(Color.red, 2).OnComplete(() =>
        {
            IsPaint = false;
            newGameObject.transform.position = _startPoint.position;
            CanSpawnNew = true;
            newGameObject.GetComponent<Renderer>().material.color = Color.white; 
            _robot.DORotate(new Vector3(-90, 0, -90), 1);
            _robot2.DORotate(new Vector3(-90, 0, 90), 1);
            _text.text = "Обработано моделей : " + Counter.ToString(); 
        });
    }
}
