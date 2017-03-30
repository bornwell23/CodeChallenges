#include <iostream>
using namespace std;

#define NUMBER_OF_DOORS 100 //This can be changed to any number (with regard to computational power of your system), and the program will still be functional
bool doors[NUMBER_OF_DOORS] = {}; // this is global for the sake of simplicity, but obviously would not exist in a more complex program

void runPassThroughs(){
	for(int i=0;i<NUMBER_OF_DOORS;++i){
		for(int j=0;j<NUMBER_OF_DOORS;++j){
			if((i+1)%(j+1)){
				doors[i] = !doors[i];
			}
		}
	}
}

void printAllDoors(){
	cout << "The state of all doors is as follows:" << endl;
	for(int i=0;i<NUMBER_OF_DOORS;++i){
		cout << "Door " << i+1 << " is " << (doors[i]?"open":"closed") << endl;
	}
	cout << endl; //this is simply for better looking output if something is printed after this function
}

void printDoors(bool open){
	cout << "The following doors are " << (open?"open":"closed") << ":" << endl;
	for(int i = 0;i<NUMBER_OF_DOORS;++i){
		if(doors[i]==open){
			cout << i+1 << " ";
		}
	}
	cout << endl; //this is simply for better looking output if something is printed after this function
}

int main(int argc, char** argv){
	cout << "Begin " << NUMBER_OF_DOORS << " Doors program" << endl;
	cout << "Running pass throughs as explained in tasks.txt..." << endl;
	runPassThroughs();
	//printAllDoors(); //this function is much more verbose, and is commented in case the functionality is requested
	printDoors(true);
	cout << "End " << NUMBER_OF_DOORS << " Doors program" << endl;
	return 0;
}