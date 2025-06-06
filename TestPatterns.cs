// Примерный файл C# для Unity: паттерн Стратегия + VContainer, с разделением View и Controller и событиями
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

public interface IButtonStrategy
{
    void Execute();
}

public class JumpStrategy : IButtonStrategy
{
    public void Execute()
    {
        Debug.Log("Выполняется прыжок!");
    }
}

public class ShootStrategy : IButtonStrategy
{
    public void Execute()
    {
        Debug.Log("Выполняется выстрел!");
    }
}

public class ButtonHandler
{
    private IButtonStrategy _strategy;

    public void SetStrategy(IButtonStrategy strategy)
    {
        _strategy = strategy;
    }

    public void ExecuteCurrentStrategy()
    {
        _strategy?.Execute();
    }
}

// Контроллер логики, реализующий IInitializable и IDisposable , пример использования паттерна стратегия(но в реальных условиях выглядеть будет избыточно),
// контроллер который реагирует на действия вью и можем запустить так же кроме какого-то сервиса еще апдейт состояния модели
public class GameController : IInitializable, IDisposable
{
    private readonly ButtonHandler _buttonHandler;
    private readonly IButtonStrategy _jumpStrategy;
    private readonly IButtonStrategy _shootStrategy;
    private readonly GameView _view;

    public GameController(ButtonHandler buttonHandler, JumpStrategy jump, ShootStrategy shoot, GameView view)
    {
        _buttonHandler = buttonHandler;
        _jumpStrategy = jump;
        _shootStrategy = shoot;
        _view = view;
    }

    public void Initialize()
    {
        _view.OnJump += OnJumpPressed;
        _view.OnShoot += OnShootPressed;
    }

    public void Dispose()
    {
        _view.OnJump -= OnJumpPressed;
        _view.OnShoot -= OnShootPressed;
    }

    private void OnJumpPressed()
    {
        _buttonHandler.SetStrategy(_jumpStrategy);
        _buttonHandler.ExecuteCurrentStrategy();
    }

    private void OnShootPressed()
    {
        _buttonHandler.SetStrategy(_shootStrategy);
        _buttonHandler.ExecuteCurrentStrategy();
    }
}

// View слой, обрабатывающий нажатия UI , + что это вью можем закинуть на любой ui и никак не связаны с бизнес логикой
public class GameView : MonoBehaviour
{
    [SerializeField] 
    private Button jumpButton;
    
    [SerializeField] 
    private Button shootButton;

    public event Action OnJump;
    public event Action OnShoot;

    private void OnEnable()
    {
        jumpButton.onClick.AddListener(InvokeJump);
        shootButton.onClick.AddListener(InvokeShoot);
    }

    private void OnDisable()
    {
        jumpButton.onClick.RemoveListener(InvokeJump);
        shootButton.onClick.RemoveListener(InvokeShoot);
    }

    private void InvokeJump()
    {
        OnJump?.Invoke();
    }

    private void InvokeShoot()
    {
        OnShoot?.Invoke();
    }
}

// вешается на монобех , который является рутом в префабе или на сцене , чтобы внедрить завивисмоти необходимые для работы
public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] 
    private GameView gameView;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(gameView);
        builder.Register<ButtonHandler>(Lifetime.Singleton);
        builder.Register<JumpStrategy>(Lifetime.Singleton);
        builder.Register<ShootStrategy>(Lifetime.Singleton);
        builder.Register<GameController>(Lifetime.Singleton);
    }
}
