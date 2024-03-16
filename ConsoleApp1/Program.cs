new Program().Run();

public partial class Program
{
    private readonly Entity _player = new(new(50), new(11));
    private readonly Entity _enemy = new(new(50), new(10));

    private bool _isPlayerTurn = true;
    
    private void Run()
    {
        if (_enemy.Health.CurrentHealth == 0)
        {
            Console.WriteLine("Победа!");
            Restart();
        }
        else if (_player.Health.CurrentHealth == 0)
        {
            Console.WriteLine("Проигрыш!");
            Restart();
        }
        
        Console.WriteLine();
        Console.WriteLine($"Здоровье игрока: {_player.Health.CurrentHealth}");
        Console.WriteLine($"Здоровье врага: {_enemy.Health.CurrentHealth}");
        Console.WriteLine();
        
        if (_isPlayerTurn) PlayerTurn();
        else EnemyTurn();
    }

    private void Restart()
    {
        Console.ReadLine();
        Console.Clear();
        new Program().Run();
    }

    private void PlayerTurn()
    {
        Console.WriteLine("Выбирите дейтвие: атака, исцеление");
        
        switch (Console.ReadLine())
        {
            case "атака": _player.Attaker.Attack(_enemy.Health);
                break;
            case "исцеление": _player.Health.Healing(5);
                break;
            default: 
                Console.WriteLine("Такого действия нет!");
                PlayerTurn();
                break;
        }
        
        _isPlayerTurn = false;
        Run();
    }

    private void EnemyTurn()
    {
        Console.WriteLine("Ход врага!");
        
        if (_enemy.Health.CurrentHealth >= _player.Health.CurrentHealth)
        {
            if (_enemy.Health.CurrentHealth <= 10) _enemy.Health.Healing(6);
            else _enemy.Attaker.Attack(_player.Health);
        }
        else
        {
            if (_enemy.Health.CurrentHealth >= 15) _enemy.Attaker.Attack(_player.Health);
            else _enemy.Health.Healing(6);
        }
        
        _isPlayerTurn = true;
        Run();
    }
}

public class Health
{
    public int CurrentHealth {get; private set;}
    private readonly int _maxHealth;
    
    public Health(int maxHealth)
    {
        _maxHealth = maxHealth;
        CurrentHealth = _maxHealth;
    }

    public void Healing(int value)
    {
        int newHealth = CurrentHealth - value;
        CurrentHealth = newHealth >= _maxHealth ? _maxHealth : newHealth;
        
        Console.WriteLine("Исцеление!");
    }

    public void TakeDamage(int damage)
    {
        int newHealth = CurrentHealth - damage;
        CurrentHealth = newHealth <= 0 ? 0 : newHealth;
        
        Console.WriteLine($"Был нанесён урон {damage}!");
    }
}

public class Attaker
{
    private readonly int _maxDamage;
    
    public Attaker(int maxDamage) => _maxDamage = maxDamage;
    public void Attack(Health health) => health.TakeDamage(new Random().Next(_maxDamage));
}

public class Entity
{
    public readonly Health Health;
    public readonly Attaker Attaker;

    public Entity(Health health, Attaker attaker)
    {
        Health = health;
        Attaker = attaker;
    }
}