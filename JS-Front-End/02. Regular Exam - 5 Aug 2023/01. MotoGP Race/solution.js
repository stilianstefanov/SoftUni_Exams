function solve(input) {
    let drivers = [];
    const maxFuel = 100;
    const numberOfDrivers = Number(input.shift());

    for (let index = 0; index < numberOfDrivers; index++) {
        let [name, fuel, position] = input.shift().split('|');
        if(Number(fuel) > maxFuel) {
            fuel = 100;
        }

        let newDriver = {
            name,
            fuel: Number(fuel),
            position: Number(position)
        };
        drivers.push(newDriver);
    }

    for (const commandInfo of input) {
        let [command, ...info] = commandInfo.split(' - ');

        if (command === 'StopForFuel') {
            let driverName = info[0];
            let minimumFuel = Number(info[1]);
            let changedPosition = Number(info[2]);
            
            let currentDriver = drivers.find(d => d.name === driverName);
            if (currentDriver.fuel < minimumFuel) {
                currentDriver.position = changedPosition;
                console.log(`${driverName} stopped to refuel but lost his position, now he is ${changedPosition}.`);

            } else {
                console.log(`${driverName} does not need to stop for fuel!`);

            }

        } else if (command === 'Overtaking') {
            let driver1Name = info[0];
            let driver2Name = info[1];    

            let driver1 = drivers.find(d => d.name === driver1Name);
            let driver2 = drivers.find(d => d.name === driver2Name);

            if (driver1.position < driver2.position) {
                let swappedPosition = driver1.position;
                driver1.position = driver2.position;
                driver2.position = swappedPosition;
                console.log(`${driver1Name} overtook ${driver2Name}!`);

            } 

        } else if (command === 'EngineFail') {
            let driverName = info[0];
            let lapsLeft = Number(info[1]);
            let currentDriver = drivers.find(d => d.name === driverName);

            drivers.splice(drivers.indexOf(currentDriver), 1);
            console.log(`${driverName} is out of the race because of a technical issue, ${lapsLeft} laps before the finish.`);

        } else {
            let output = [];
            for (const driver of drivers) {
                let currentDriverOutput = `${driver.name}\n  Final position: ${driver.position}`;
                output.push(currentDriverOutput);
            }
            console.log(output.join('\n'));
        }
    }
}

solve(["4",

"Valentino Rossi|100|1",

"Marc Marquez|90|3",

"Jorge Lorenzo|80|4",

"Johann Zarco|80|2",

"StopForFuel - Johann Zarco - 90 - 5", "Overtaking - Marc Marquez - Jorge Lorenzo", "EngineFail - Marc Marquez - 10", "Finish"])