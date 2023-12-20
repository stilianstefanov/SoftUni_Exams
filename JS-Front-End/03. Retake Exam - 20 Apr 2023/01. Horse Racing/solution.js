function solve(input) {
    let horses = input.shift().split('|');

    for (const commandInfo of input) {
        let [command, ...info] = commandInfo.split(' ');
        
        if (command === 'Retake') {
            let overtaikingHorse = info[0];
            let overtakenHorse = info[1];

            let indexOfOvertaking = horses.indexOf(overtaikingHorse);
            let indexOfOvertaken = horses.indexOf(overtakenHorse);

            if (indexOfOvertaking < indexOfOvertaken) {
                horses[indexOfOvertaken] = overtaikingHorse;
                horses[indexOfOvertaking] = overtakenHorse;
                console.log(`${overtaikingHorse} retakes ${overtakenHorse}.`);
            }

        } else if (command === 'Trouble') {
            let horse = info[0];
            let indexOfHorse = horses.indexOf(horse);
            
            if (indexOfHorse > 0) {
                let indexOfOvertaking = indexOfHorse - 1;
                let overtakingHorse = horses[indexOfOvertaking];

                horses[indexOfHorse] = overtakingHorse;
                horses[indexOfOvertaking] = horse;
                console.log(`Trouble for ${horse} - drops one position.`);
            }

        } else if (command === 'Rage') {
            let horse = info[0];
            let indexOfHorse = horses.indexOf(horse);

            if (indexOfHorse === horses.length - 1 || indexOfHorse === horses.length - 2) {
                horses.splice(indexOfHorse, 1);
                horses.push(horse);

            } else {
                let overtakenHorses = horses.splice(indexOfHorse + 1, 2);
                horses.splice(indexOfHorse, 0, ...overtakenHorses);
            }
            console.log(`${horse} rages 2 positions ahead.`);

        } else if (command === 'Miracle') {
            let horse = horses.shift();
            horses.push(horse);
            console.log(`What a miracle - ${horse} becomes first.`);

        } else {
            console.log(horses.join('->'));
            console.log(`The winner is: ${horses.pop()}`);
            break;
        }
    }
}

solve(['Fancy|Lilly',

'Retake Lilly Fancy',

'Trouble Lilly',

'Trouble Lilly',

'Finish',

'Rage Lilly'])