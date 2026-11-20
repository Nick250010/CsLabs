#include <iostream>
#include <fstream>
#include <string>
#include <stdexcept>

using namespace std;


class Person
{
private:
    string name,ClassOfPerson;
    short lvl;
    int experience,health = maxHp,strength,protection,gold;
    const int maxHp;
    
public:

    Person(string name,string ClassOfPerson,int maxHp,short lvl,int experience,int gold,int strength,int protection)
        : name(name), ClassOfPerson(ClassOfPerson), maxHp(maxHp), lvl(lvl), experience(experience), gold(gold), strength(strength), protection(protection){}

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
            throw invalid_argument("Лечение должно быть строго больше нуля")ж
        }
        health += heal;
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
    int protection = stoi(line.substr(index1+1,index2-index1-1));
    Person person(name,ClassOfPerson,mxHp,lvl,experience,gold,strength,protection);
    return person;
}


void Menu(Person& pers){
    int choice,amount;

    do
    {
        cout << "\n===== МЕНЮ =====" << endl;
        cout << "1. Показать характеристики" << endl;
        cout << "2. Получить урон" << endl;
        cout << "3. Восстановить здоровье" << endl;
        cout << "4. Получить опыт" << endl;
        cout << "5. Получить золото" << endl;
        cout << "6. Потратить золото" << endl;
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
            case 3:
                cout << "Введите количество лечения: ";
                if (!(cin >> amount))
                    throw invalid_argument("Нужно ввести число!");
                pers.Heal(amount);
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


int main()
{
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