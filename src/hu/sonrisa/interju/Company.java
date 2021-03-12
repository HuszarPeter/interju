package hu.sonrisa.interju;

import java.util.ArrayList;
import java.util.List;

// Feladat első része:
// Modellez objektum orientáltan egy átlagos cég hierarchiát, ahol:
// - Egynél több alkalmazott dolgozik
// - minden alkalmazott kap fizetést
// - egy alkalmazott indulásnak lehet dolgozó, vagy manager
// - a dolgozóknak nincsenek beosztottaik
// - managereknek egy vagy több beosztottja is lehet
// - manager beosztottja lehet másik manager is

// Második rész:
// készíts egy osztályt amelyik tartalmazza az egész cég hierarchiáját
// ennek az osztálynak legyen egy metódusa amelyik rekurzív bejárással 
// összeszámolja a cégben dolgozó összes alkalmazott fizetését, majd a teljes 
// összeget visszaadja.

public class Company {

     public static void main(String[] args) {

        Company company = new Company();
        company.test();
     }


     private void test() {
        List<Employee> employeeList = new ArrayList<>();

        List<Employee> managerEmployeeList = new ArrayList<>();
        managerEmployeeList.add(new Worker(1));
        managerEmployeeList.add(new Worker(2));
        managerEmployeeList.add(new Worker(3));
        managerEmployeeList.add(new Manager(12, null));

        employeeList.add(new Manager(10, managerEmployeeList));
        employeeList.add(new Worker(5));
        employeeList.add(new Worker(7));
        employeeList.add(new Worker(6));

        System.out.println("sum: " + sumSalary(employeeList));
    }

    public int sumSalary(List<Employee> employeeList) {
        if (employeeList == null) 
            return 0;
        int sum = 0;
        for (Employee employee : employeeList) {
            sum += employee.getSalary();
            if (employee instanceof Manager) {
                sum += sumSalary (((Manager) employee).getEmployeeList());
            }
        }
        return sum;
    }

    private abstract class Employee {

        private int salary;

        public Employee(int salary) {
            this.salary = salary;
        }

        public int getSalary() {
            return salary;
        }

        public void setSalary(int salary) {
            this.salary = salary;
        }

    }

    private class Manager extends Employee {

        private List<Employee> employeeList;

        public Manager (int salary, List<Employee> employeeList) {
            super(salary);
            this.employeeList = employeeList;
        }

        public List<Employee> getEmployeeList() {
            return employeeList;
        }

        public void setEmployeeList(List<Employee> employeeList) {
            this.employeeList = employeeList;
        }

    }

    private class Worker extends Employee {

        public Worker(int salary) {
            super(salary);
        }

    }

}