using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaryBook : MonoBehaviour
{
    [SerializeField] private float _turnSpeed;
    [SerializeField] private DiarySheet _firstSheet;
    [SerializeField] private DiarySheet _defaultSheet;
    [SerializeField] private DiarySheet _lastSheet;
    [SerializeField] private float _closedAngle = -20f;
    [SerializeField] private float _opendAngle = -160f;
    [Header("Sheets Data")]
    [SerializeField] private List<SheetData> _currentSheetsData;
    [Header("Navigation Buttons")]
    [SerializeField] private Button _nextPageButton;
    [SerializeField] private Button _previousPageButton;

    private bool _canTurn;
    private bool _isTurning;

    private float _currentAngle;
    private int _currentPageIndex = 0;
    private DiarySheet _targetSheet;
    private DiarySheet _hideSheet;
    private float _targetAngle;

    public void Initialize()
    {
        SubscribeToEvents();

        _firstSheet.SetData(_currentSheetsData[0]);

        CheckNavigationButtons();
    }

    private void Update()
    {  
        SheetTurnHandler();
    }  

    public void AddSheet(SheetData newData)
    {
        _currentSheetsData.Add(newData);
    }

    private void SheetTurnHandler()
    {
        if (!_canTurn)
            return;

        TurnSheet(_targetSheet, _targetAngle,
                () =>
                {
                    Debug.Log("Середина");
                },
                () =>
                {
                    Debug.Log("Перевернули");
                    _canTurn = false;
                    _isTurning = false;
                   if (_hideSheet!= null)
                    {
                        _hideSheet.gameObject.SetActive(false);
                        _hideSheet = null;
                    }
                    CheckNavigationButtons();
                });
    }

    private void TurnSheet(DiarySheet sheet, float targetAngle,Action OnCenter, Action OnComplete)
    {
        _isTurning = true;
        float t = _turnSpeed * Time.deltaTime;
        _currentAngle = Mathf.MoveTowards(_currentAngle, targetAngle, t);

        _currentAngle = Math.Clamp(_currentAngle, _opendAngle,_closedAngle);

        sheet.transform.localRotation = Quaternion.Euler(0, 0, _currentAngle);

        if (Mathf.Abs(_currentAngle - targetAngle) < 0.01f)
        {
            OnComplete?.Invoke();
        }
    }

    #region >>> NAVIGATION

    private void TryOpenNextPage()
    {
        if (_isTurning)
            return;

        _currentAngle = _opendAngle;       

        if (_currentSheetsData.Count > 2)
        {
            if (_currentPageIndex == 0)
            {
                _currentPageIndex++;
                _targetSheet = _firstSheet;
                _defaultSheet.gameObject.SetActive(true);
                _defaultSheet.SetData(_currentSheetsData[_currentPageIndex]);
            }
            else if (_currentPageIndex == _currentSheetsData.Count - 1)
            {
                _currentPageIndex++;
                _targetSheet = _lastSheet;
                _defaultSheet.gameObject.SetActive(false);
                _hideSheet = _firstSheet;
            }
            else
            {               
                _currentPageIndex++;
                _targetSheet = _defaultSheet;
                _lastSheet.gameObject.SetActive(true);
                _lastSheet.SetData(_currentSheetsData[_currentPageIndex]);
            }

        }
        else if(_currentSheetsData.Count == 2)
        {
            if(_currentPageIndex == 0)
            {
                _currentPageIndex++;
                _targetSheet = _firstSheet;
                _lastSheet.gameObject.SetActive(true);
                _lastSheet.SetData(_currentSheetsData[_currentPageIndex]);              
            }
            else
            {
                _currentPageIndex++;
                _targetSheet = _lastSheet;
                _hideSheet = _firstSheet;
            }
        }
        else
        {
            _currentPageIndex++;
            _targetSheet = _firstSheet;           
                            
        }

        _targetAngle = _closedAngle;
        _canTurn = true;
    }

    private void TryOpenPreviousPage()
    {
        if (_isTurning)
            return;

        _currentAngle = _closedAngle;

        if (_currentSheetsData.Count > 2)
        {
            if (_currentPageIndex == 1)
            {
                _currentPageIndex--;
                _targetSheet = _firstSheet;
                _firstSheet.SetData(_currentSheetsData[_currentPageIndex]);
                _hideSheet = _lastSheet;
            }
            else if (_currentPageIndex == _currentSheetsData.Count)
            {
                _currentPageIndex--;
                _targetSheet = _lastSheet;
                _firstSheet.gameObject.SetActive(true);
                _lastSheet.SetData(_currentSheetsData[_currentPageIndex]);
            }
            else
            {
                _currentPageIndex--;
                _targetSheet = _defaultSheet;
                _defaultSheet.gameObject.SetActive(true);
                _defaultSheet.SetData(_currentSheetsData[_currentPageIndex]);              
                _hideSheet = _lastSheet;
            }
        }
        else if(_currentSheetsData.Count  == 2)
        {
            if (_currentPageIndex == 1)
            {
                _currentPageIndex--;
                _targetSheet = _firstSheet;
                _firstSheet.SetData(_currentSheetsData[_currentPageIndex]);
               
                _hideSheet = _lastSheet;
            }
            else if (_currentPageIndex == 2)
            {
                _currentPageIndex--;
                _targetSheet = _lastSheet;
                _firstSheet.gameObject.SetActive(true);
                _lastSheet.SetData(_currentSheetsData[_currentPageIndex]);
            }
        }
        else
        {
            _currentPageIndex--;
            _targetSheet = _firstSheet;
            _hideSheet = _lastSheet;
        }

        _targetAngle = _opendAngle;
        _canTurn = true;
    }

    private void CheckNavigationButtons()
    {
        if (_currentPageIndex <= 0)
        {
            _previousPageButton.gameObject.SetActive(false);
        }
        else
        {
            _previousPageButton.gameObject.SetActive(true);
        }

        if (_currentPageIndex >= _currentSheetsData.Count)
        {
            _nextPageButton.gameObject.SetActive(false);
        }
        else
        {
            _nextPageButton.gameObject.SetActive(true);
        }
    }

    #endregion
    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        _nextPageButton.onClick.AddListener(OnNextPageButtonClicked);
        _previousPageButton.onClick.AddListener(OnPreviousPageButtonClicked);
    }

    private void UnSubscribeToEvents()
    {
        _nextPageButton.onClick.RemoveAllListeners();
        _previousPageButton.onClick.RemoveAllListeners();

    }

    private void OnNextPageButtonClicked()
    {
        TryOpenNextPage();
    }

    private void OnPreviousPageButtonClicked()
    {
        TryOpenPreviousPage();
    }

    #endregion

    public void ResetToFirstPage()
    {
        // Сбрасываем индекс текущей страницы
        _currentPageIndex = 0;

        // Отключаем возможность перелистывания во время сброса
        _canTurn = false;

        // Сбрасываем угол поворота
        _currentAngle = 0f;

       

        // Устанавливаем данные для первого листа
        _firstSheet.SetData(_currentSheetsData[0]);

        // Сбрасываем целевые листы
        _targetSheet = null;
        _hideSheet = null;

        // Применяем правильную ротацию для первого листа
        _firstSheet.transform.localRotation = Quaternion.Euler(0, 0, _opendAngle);
        _defaultSheet.transform.localRotation = Quaternion.Euler(0,0, _opendAngle);
        _lastSheet.transform.localRotation = Quaternion.Euler(0, 0, _opendAngle);

        // Деактивируем все листы
        _firstSheet.gameObject.SetActive(true);
        _defaultSheet.gameObject.SetActive(false);
        _lastSheet.gameObject.SetActive(false);

        // Обновляем состояние кнопок навигации
        CheckNavigationButtons();

        Debug.Log("Сброс до первой страницы выполнен");
    }

    private void OnDestroy()
    {
        UnSubscribeToEvents();
    }
}
