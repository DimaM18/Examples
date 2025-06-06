// Пример C# скрипта для Unity с VContainer, UniTask и DI сервисом
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;
using System;
using Newtonsoft.Json;


// UserDto - пример дто , куда сможем запарсить ответ от 
[Serializable]
public class UserDto
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("age")]
    public int Age;

    public static UserDto FromJson(string json)
    {
        return JsonConvert.DeserializeObject<UserDto>(json);
    }
}


public interface IUserView
{
    void SetUserText(string text);
}

public class UserView : MonoBehaviour, IUserView
{
    [SerializeField] 
    private Text userTextUI;

    public void SetUserText(string text)
    {
        if (userTextUI != null)
        {
            userTextUI.text = text;
        }
        else
        {
            Debug.Log("UserView: " + text);
        }
    }
}

// сервис для получения данных юзера
public interface IUserService
{
    UniTask<UserDto> GetUserAsync();
}

public class UserService : IUserService
{
    public async UniTask<UserDto> GetUserAsync()
    {
        const string jsonResponse = "{\"name\":\"Иван Иванов\",\"age\":30}";

        await UniTask.Delay(1000); // имитация сетевого запроса

        var user = UserDto.FromJson(jsonResponse);
        return user;
    }
}

// UserController.cs
public class UserController : IStartable
{
    private readonly IUserService _userService;
    private readonly IUserView _userView;

    public UserController(IUserService userService, IUserView userView)
    {
        _userService = userService;
        _userView = userView;
    }

    async void IStartable.Start()
    {
        try
        {
            var user = await _userService.GetUserAsync();

            var text = $"Имя: {user.Name}, Возраст: {user.Age}";
            _userView.SetUserText(text);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}

public class ProjectLifetimeScope : LifetimeScope
{
    [SerializeField] 
    private UserView _userViewPrefab;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<IUserService, UserService>(Lifetime.Singleton);
        builder.RegisterComponent(_userViewPrefab).As<IUserView>();
        builder.RegisterEntryPoint<UserController>();
    }
}
