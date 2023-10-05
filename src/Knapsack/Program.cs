namespace Knapsack;
class Program
{
  static void Main(string[] args)
  {
    // Jakieś rozsądne 1 1 0 0 1 0
    int maxWeight = 190;
    List<int> weights = new List<int> { 56, 59, 80, 64, 75, 17 };
    List<int> values = new List<int> { 50, 50, 64, 55, 50, 5 };
    List<Item> solution = new List<Item>();
    List<Item> items = new List<Item>();
    for (int i = 0; i < weights.Count; i++)
      items.Add(new Item(weights[i], values[i]));
    KnapsackProblem kp = new KnapsackProblem(items, maxWeight);

    // Z wykładu
    // int maxWeight = 5;
    // List<int> weights = new List<int> { 1, 2, 5, 4 };
    // List<int> values = new List<int> { 2, 3, 6, 3 };
    // List<Item> items = new List<Item>();
    // for (int i = 0; i < weights.Count; i++)
    //   items.Add(new Item(weights[i], values[i]));
    // KnapsackProblem kp = new KnapsackProblem(items, maxWeight);
    int B = 0;
    int currentLevel = -1; // k
    int bestSolution = -1; // PROF
    int finalWeight = -1;

    do
    {
      while ((B = kp.Bound(currentLevel)) <= bestSolution && currentLevel != -1)
      {
        currentLevel = kp.GetLastPackedItemIndex(currentLevel);

        if (currentLevel > -1)
        {
          kp.Items[currentLevel].IsPacked = false;
          kp.Weight -= kp.Items[currentLevel].Weight;
          kp.Value -= kp.Items[currentLevel].Value;
        }
      };

      if (currentLevel > -1 || bestSolution == -1)
      {
        kp.Weight = kp.boundedWeight;
        kp.Value = kp.boundedValue;
        currentLevel = kp.boundedLevel;
        if (currentLevel >= kp.Items.Count)
        {
          bestSolution = kp.Value;
          finalWeight = kp.Weight;
          currentLevel = kp.Items.Count - 1;
          solution = new List<Item>();
          foreach (var item in kp.Items)
            solution.Add(new Item(item));
        }
        else
          kp.Items[currentLevel].IsPacked = false;
      }
    } while (currentLevel != -1);

    Console.WriteLine($"Packed items weight: {finalWeight}");
    Console.WriteLine($"Packed items value: {bestSolution}");
    foreach (var item in solution)
      Console.WriteLine($"Item: (w: {item.Weight}, v: {item.Value}) | {item.IsPacked}");
  }
}

public class KnapsackProblem
{
  public int MaxWeight { get; }
  public List<Item> Items { get; private set; }
  public int Value;
  public int Weight;
  public int boundedValue;
  public int boundedWeight;
  public int boundedLevel;

  public KnapsackProblem(List<Item> items, int maxWeight)
  {
    Weight = 0;
    Value = 0;
    MaxWeight = maxWeight;
    Items = new List<Item>();
    foreach (var item in items)
      Items.Add(new Item(item));
  }

  public int GetWeight()
  {
    // return boundedItems.Where(item => item.IsPacked).Sum(item => item.Weight);
    return Weight;
  }

  public int GetValue()
  {
    // return boundedItems.Where(item => item.IsPacked).Sum(item => item.Value);
    return Value;
  }

  private void PackItem(Item item)
  {
    // We assume proper reference
    item.IsPacked = true;
  }

  private void UnpackItem(Item item)
  {
    item.IsPacked = false;
  }

  public int Bound(int k)
  {
    bool IsValidBound = true;
    boundedValue = GetValue();
    boundedWeight = GetWeight();
    boundedLevel = k + 1;
    while (boundedLevel < Items.Count && IsValidBound)
    {
      IsValidBound = boundedWeight + Items[boundedLevel].Weight <= MaxWeight;

      if (IsValidBound)
      {
        boundedValue += Items[boundedLevel].Value;
        boundedWeight += Items[boundedLevel].Weight;
        Items[boundedLevel].IsPacked = true;
        boundedLevel++;
      }
    }

    return IsValidBound ? boundedValue : boundedValue + (MaxWeight - boundedWeight) * Items[boundedLevel].Value / Items[boundedLevel].Weight;
  }

  public int GetLastPackedItemIndex(int maxIndex)
  {
    int index = -1;
    for (int j = 0; j <= maxIndex; j++)
      if (Items[j].IsPacked)
        index = j;
    return index;
  }
}

public class Item
{
  public int Weight { get; }
  public int Value { get; }
  public bool IsPacked;

  public Item(Item item)
  {
    Weight = item.Weight;
    Value = item.Value;
    IsPacked = item.IsPacked;
  }

  public Item(int weight, int value)
  {
    Weight = weight;
    Value = value;
    IsPacked = false;
  }
}