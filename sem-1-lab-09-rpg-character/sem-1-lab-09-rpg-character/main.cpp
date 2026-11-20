#include <iostream>
#include <fstream>
#include <string>
#include <stdexcept>
#include <limits>
#include <vector>
#include <cassert>

using namespace std;


class Person
{
private:
    string name,ClassOfPerson;
    short lvl;
    int experience,health,strength,protection,gold;
    const int maxHp;
    vector<string> inventory;
    
public:

    Person(string name,string ClassOfPerson,int maxHp,short lvl,int experience,int gold,int strength,int protection)
        : name(name), ClassOfPerson(ClassOfPerson), maxHp(maxHp), lvl(lvl), experience(experience), gold(gold), strength(strength), protection(protection), health(maxHp){


            if (name.empty())
                throw invalid_argument("Имя не может быть пустым!");

            if (maxHp <= 0 || lvl < 1 || experience < 0 ||
                gold < 0 || strength < 0 || protection < 0)
                throw invalid_argument("Некорректные характеристики!");
        }

    void ShowInfo()
    {
        cout << "\n===== ХАРАКТЕРИСТИКИ =====" << endl;
        cout << "Имя: " << name << endl;
        cout << "Класс: " << ClassOfPerson << endl;
        cout << "Уровень: " << lvl << endl;
        cout << "Опыт: " << experience << endl;
        cout << "Здоровье: " << health << "/" << maxHp << endl;
        cout << "Сила: " << strength << endl;
        cout << "Защита: " << protection << endl;
        cout << "Золото: " << gold << endl;
    }

    void TakeDamage(int damage){
        if (damage <= 0){
            throw invalid_argument("Урон должен быть больше нуля!");
        }
        health-= damage;

        if(health < 0){ health = 0;}
    }

    void Heal(int heal){
        if(heal <= 0){
            throw invalid_argument("Лечение должно быть строго больше нуля");
        }
        health += heal;

        if(health > maxHp){
            health = maxHp;
        }
    }
    void GetExperience(int exp){
        if(exp <= 0){
            throw invalid_argument("Полученный опыт может быть строго больше нуля");
        }
        experience+=exp;

        while(experience>= 100){
            experience-=100;
            lvl+=1;
        }
    }

    void GetGold(int amount){
        if(amount <= 0){
            throw invalid_argument("Количество золота должно быть больше нуля");
        }
        gold+=amount;
    }

    void SpendGold(int amount){
        if(amount > gold){
            cout << "Ты не можешь потратить золота больше чем у тебя есть\n";
            throw runtime_error("Недостаточно золота!");
        }else if(amount <= 0) {
            throw invalid_argument("Нельзя тратить отрицательное количесво золота");
        }
        else{
            gold -= amount;
        }
    }

    void AddItem(string thing){
        if (thing.empty()) {
            throw invalid_argument("Нельзя добавить пустой предмет!");
        }

        inventory.push_back(thing);
    }

    void RemoveItem(string thing){
        for (int i = 0; i < inventory.size(); i++){
            if (inventory[i] == thing){
                inventory.erase(inventory.begin() + i);
                return;
            }
        }

        throw runtime_error("Такого предмета нет в инвентаре!");
    }

    void ShowInventory(){
        cout << "\n===== ИНВЕНТАРЬ =====\n";

        if (inventory.empty()){
            cout << "Инвентарь пуст!\n";
            return;
        }

        for (int i = 0; i < inventory.size(); i++){
            cout << i + 1 << ". " << inventory[i] << endl;
        }
    }

    int GetHealth() const {
        return health;
    }
};

Person ReadStats(string nameOFfile){

    ifstream file(nameOFfile);
    if(!file.is_open()){
        throw runtime_error("Ошибка открытия файла!");
    }
    string line;
    getline(file,line);
    int index1 = line.find("|");
    string name = line.substr(0,index1);
    int index2 = line.find("|",index1+1);
    string ClassOfPerson = line.substr(index1+1,index2-index1-1);
    index1 = line.find("|",index2+1);
    int mxHp = stoi(line.substr(index2+1,index1-index2-1));
    index2 = line.find("|",index1+1);
    short lvl = stoi(line.substr(index1+1,index2-index1-1));
    index1 = line.find("|",index2+1);
    int experience = stoi(line.substr(index2+1,index1-index2-1));
    index2 = line.find("|",index1+1);
    int gold = stoi(line.substr(index1+1,index2-index1-1));
    index1 = line.find("|",index2+1);
    int strength = stoi(line.substr(index2+1,index1-index2-1));
    index2 = line.find("|",index1+1);
    int protection = stoi(line.substr(index1 + 1));
    Person person(name,ClassOfPerson,mxHp,lvl,experience,gold,strength,protection);
    return person;
}


void Menu(Person& pers){
    int choice,amount;
    string thing;

    do
    {
        cout << "\n===== МЕНЮ =====" << endl;
        cout << "1. Показать характеристики" << endl;
        cout << "2. Получить урон" << endl;
        cout << "3. Восстановить здоровье" << endl;
        cout << "4. Получить опыт" << endl;
        cout << "5. Получить золото" << endl;
        cout << "6. Потратить золото" << endl;
        cout << "7. Положить вещь в инвентарь" << endl;
        cout << "8. Достать вещь из инвентарь" << endl;
        cout << "9. Посмотреть инвентарь" << endl;
        cout << "0. Выход" << endl;
        cout << "Выберите действие: ";

        if(!(cin >> choice)){
            cin.clear();;
            cin.ignore(numeric_limits<streamsize>::max(), '\n');
            cout << "Введите число!" << endl;
            continue;
        }

        try{
            switch (choice)
            {
            case 1:
                pers.ShowInfo();
                break;
            
            case 2:
                cout << "Введите урон: ";
                if(!(cin >> amount)){
                    throw invalid_argument("Нужно ввести число!");
                }
                pers.TakeDamage(amount);
                break;
            case 3:
                cout << "Введите количество лечения: ";
                if (!(cin >> amount))
                    throw invalid_argument("Нужно ввести число!");
                pers.Heal(amount);
                break;
            case 4:
                cout << "Введите количество полученного опыта: ";
                if (!(cin >> amount))
                    throw invalid_argument("Нужно ввести число!");
                pers.GetExperience(amount);
                break;
            case 5:
                cout << "Введите количество полученного золота: ";
                if (!(cin >> amount)){
                    throw invalid_argument("Нужно ввести число!");
                }
                pers.GetGold(amount);
                break;

            case 6:
                cout << "Введите количество полученного золота: ";
                if (!(cin >> amount)){
                    throw invalid_argument("Нужно ввести число!");
                }
                pers.SpendGold(amount);
                break;
            case 7:
                cout << "Введите название предмета: ";
                cin.ignore(numeric_limits<streamsize>::max(), '\n');
                getline(cin, thing);
                pers.AddItem(thing);
                break;

            case 8:
                cout << "Введите название предмета для удаления: ";
                cin.ignore(numeric_limits<streamsize>::max(), '\n');
                getline(cin, thing);
                pers.RemoveItem(thing);
                break;

            case 9:
                pers.ShowInventory();
                break;
            default:
                break;
            }
        }
        catch (const exception& e)
        {
            cout << "Ошибка: " << e.what() << endl;

            if (cin.fail())
            {
                cin.clear();
                cin.ignore(numeric_limits<streamsize>::max(), '\n');
            }
        }

    } while (choice!=0);
    
}

void runTests() {
    Person person("Алекс", "Воин", 100, 1, 0, 50, 10, 5);
    person.TakeDamage(30);
    assert(person.GetHealth() == 70);

    std::cout << "Все тесты пройдены\n";
}


int main(int argc, char* argv[]){

    if (argc > 1 && std::string(argv[1]) == "test") {
        runTests();
        return 0;
    }
    try
    {
        Person pers = ReadStats("character.txt");

        cout << "Персонаж успешно загружен!" << endl;

        Menu(pers);
    }
    catch (const exception& e)
    {
        cout << "Ошибка программы: " << e.what() << endl;
        return 1;
    }

    return 0;
}



