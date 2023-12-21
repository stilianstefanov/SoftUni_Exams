function solve(input) {
    const inputCount = Number(input.shift());
    let assignees = [];

    for (let index = 0; index < inputCount; index++) {
        let [assigneeName, taskId, title, status, points] = input.shift().split(':');

        let newTask = {
            taskId,
            title,
            status,
            points: Number(points)
        };
        
        if (assignees.some(a => a.name === assigneeName)) {
            let assignee = assignees.find(a => a.name === assigneeName);
            assignee.tasks.push(newTask);

        } else {
            let newAssignee = {
                name: assigneeName,
                tasks: []
            };
            newAssignee.tasks.push(newTask);
            assignees.push(newAssignee);
        }
    }

    for (const commandInfo of input) {
        let [command, ...info] = commandInfo.split(':');

        if (command === 'Add New') {
            let [assigneeName, taskId, title, status, points] = info;

            let assignee = assignees.find(a => a.name === assigneeName);
            if (!assignee) {
                console.log(`Assignee ${assigneeName} does not exist on the board!`);

            } else {
                let newTask = {
                    taskId,
                    title,
                    status,
                    points: Number(points)
                };
                assignee.tasks.push(newTask);
            }

        } else if (command === 'Change Status') {
            let [assigneeName, taskId, newStatus] = info;

            let assignee = assignees.find(a => a.name === assigneeName);
            if (!assignee) {
                console.log(`Assignee ${assigneeName} does not exist on the board!`);

            } else {
                let task = assignee.tasks.find(t => t.taskId === taskId);
                if (!task) {
                    console.log(`Task with ID ${taskId} does not exist for ${assigneeName}!`);

                } else {
                    task.status = newStatus;
                }
            }

        } else if (command === 'Remove Task') {
            let [assigneeName, indexStr] = info;
            let index = Number(indexStr);

            let assignee = assignees.find(a => a.name === assigneeName);
            if (!assignee) {
                console.log(`Assignee ${assigneeName} does not exist on the board!`);

            } else {
                if (index < 0 || index >= assignees.length) {
                    console.log('Index is out of range!');

                } else {
                    assignee.tasks.splice(index, 1);
                }
            }
        }
    }

    let toDoPoints = 0;
    let inProgressPoints = 0;
    let codeReviewPoints = 0;
    let donePoints = 0;

    for (const assignee of assignees) {

        for (const task of assignee.tasks) {
            if (task.status === 'ToDo') {
                toDoPoints += task.points;

            } else if (task.status === 'In Progress') {
                inProgressPoints += task.points;

            } else if (task.status === 'Code Review') {
                codeReviewPoints += task.points;

            } else if (task.status === 'Done') {
                donePoints += task.points;
            }
        }
    }

    console.log(`ToDo: ${toDoPoints}pts`);
    console.log(`In Progress: ${inProgressPoints}pts`);
    console.log(`Code Review: ${codeReviewPoints}pts`);
    console.log(`Done Points: ${donePoints}pts`);

    let notFinishedTasksPoints = toDoPoints + inProgressPoints + codeReviewPoints;
    if (donePoints >= notFinishedTasksPoints) {
        console.log('Sprint was successful!');

    } else {
        console.log('Sprint was unsuccessful...');
    }
}

solve([
    '5',
    
    'Kiril:BOP-1209:Fix Minor Bug:ToDo:3',
    
    'Mariya:BOP-1210:Fix Major Bug:In Progress:3',
    
    'Peter:BOP-1211:POC:Code Review:5',
    
    'Georgi:BOP-1212:Investigation Task:Done:2',
    
    'Mariya:BOP-1213:New Account Page:In Progress:13',
    
    'Add New:Kiril:BOP-1217:Add Info Page:In Progress:5',
    
    'Change Status:Peter:BOP-1290:ToDo',
    
    'Remove Task:Mariya:1',
    
    'Remove Task:Joro:1',
    
    ]);