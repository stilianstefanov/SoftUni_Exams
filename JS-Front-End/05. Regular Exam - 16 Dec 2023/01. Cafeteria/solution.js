function solve(input) {
    let baristas = [];
    let baristasCount = Number(input.shift());

    for (let index = 0; index < baristasCount; index++) {
        let [name, shift, coffeeTypes] = input.shift().split(' ');
        
        let newBarista = {
            name,
            shift,
            coffeeTypes: []
        };

        for (const coffeeType of coffeeTypes.split(',')) {
            newBarista.coffeeTypes.push(coffeeType);

        }

        baristas.push(newBarista);
    }

    for (const commandInfo of input) {
        let [command, ...info] = commandInfo.split(' / ');

        if (command === 'Prepare') {
            let [name, requiredShift, requiredCoffeeType] = info;

            let barista = baristas.find(b => b.name === name);

            if (barista.shift === requiredShift && barista.coffeeTypes.some(t => t === requiredCoffeeType)) {
                console.log(`${name} has prepared a ${requiredCoffeeType} for you!`);

            } else {
                console.log(`${name} is not available to prepare a ${requiredCoffeeType}.`);

            }
        } else if (command === 'Change Shift') {
            let [name, newShift] = info;

            let barista = baristas.find(b => b.name === name);

            barista.shift = newShift;
            console.log(`${name} has updated his shift to: ${newShift}`);

        } else if (command === 'Learn') {
            let [name, newCoffeeType] = info;

            let barista = baristas.find(b => b.name === name);

            if (barista.coffeeTypes.some(t => t === newCoffeeType)) {
                console.log(`${name} knows how to make ${newCoffeeType}.`);

            } else {
                barista.coffeeTypes.push(newCoffeeType);
                console.log(`${name} has learned a new coffee type: ${newCoffeeType}.`);

            }

        } else {
            for (const barista of baristas) {
                console.log(`Barista: ${barista.name}, Shift: ${barista.shift}, Drinks: ${barista.coffeeTypes.join(', ')}`);
            }
            break;
        }
    }
}



solve([
    '3',
    
    'Alice day Espresso,Cappuccino',
    
    'Bob night Latte,Mocha',
    
    'Carol day Americano,Mocha',
    
    'Prepare / Alice / day / Espresso',
    
    'Change Shift / Bob / night',
    
    'Learn / Carol / Latte',
    
    'Learn / Bob / Latte',
    
    'Prepare / Bob / night / Latte',
    
    'Closed'])