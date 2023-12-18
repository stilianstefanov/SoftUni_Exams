function solve(input) {
    let astronauts = [];
    const maximumEnergy = 200;
    const maximumOxygen = 100;
    const astronautsCount = parseInt(input.shift());

    for (let index = 0; index < astronautsCount; index++) {
        let [name, oxygen, energy] = input.shift().split(' ');
        if (Number(oxygen) > maximumOxygen) oxygen = maximumOxygen;
        if (Number(energy) > maximumEnergy) energy = maximumEnergy;
        
        let newAstronaut = {
            name,
            oxygen: Number(oxygen),
            energy: Number(energy)
        };
        astronauts.push(newAstronaut);
    }

    for (const commandInfo of input) {
        let [command, ...info] = commandInfo.split(' - ');

        if (command === 'Explore') {
            let name = info[0];
            let energyNeeded = Number(info[1]);
            let astronaut = astronauts.find(a => a.name === name);

            if (astronaut.energy >= energyNeeded) {
                astronaut.energy -= energyNeeded;
                console.log(`${name} has successfully explored a new area and now has ${astronaut.energy} energy!`);

            } else {
                console.log(`${name} does not have enough energy to explore!`);
            }

        } else if (command === 'Refuel') {
            let name = info[0];
            let amount = Number(info[1]);
            let astronaut = astronauts.find(a => a.name === name);
            let energyRecovered = Math.min(amount, maximumEnergy - astronaut.energy);
            
            astronaut.energy += energyRecovered;
            console.log(`${name} refueled their energy by ${energyRecovered}!`);

        } else if (command === 'Breathe') {
            let name = info[0];
            let amount = Number(info[1]);
            let astronaut = astronauts.find(a => a.name === name);
            let oxygenRecovered = Math.min(amount, maximumOxygen - astronaut.oxygen);

            astronaut.oxygen += oxygenRecovered;
            console.log(`${name} took a breath and recovered ${oxygenRecovered} oxygen!`);

        } else {

            for (const astronaut of astronauts) {
                console.log(`Astronaut: ${astronaut.name}, Oxygen: ${astronaut.oxygen}, Energy: ${astronaut.energy}`);
            }
            break;
        }
    }
}

solve(['3',

'John 50 120',

'Kate 80 180',

'Rob 70 150',

'Explore - John - 50',

'Refuel - Kate - 30',

'Breathe - Rob - 20',

'End'])