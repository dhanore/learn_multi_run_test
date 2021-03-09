const uri = 'api/Fitness';
let books = [];
var status = 0;
function getItems() {

    document.getElementById("curr_track").style.display = "none";

    document.getElementById("btnstoprun1").disabled = true;
    document.getElementById("btnwarn1").disabled = true;
    document.getElementById("btnstoprun2").disabled = true;
    document.getElementById("btnwarn2").disabled = true;
    document.getElementById("btnstoprun3").disabled = true;
    document.getElementById("btnwarn3").disabled = true;
    status = 0;
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function getAthletesResult(id) {
    fetch(`${uri}/${id}/result`)
        .then(response => response.json())
        .then(data => _displayShuttleNo(id, data))
        .catch(error => console.error('Unable to get items.', error));
}

var loadsecond = 0;
function getNextShuttle(id) {
    status = 1;
    fetch(uri + "/" + id)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .then(() => {
            move(loadsecond);
        })
        .catch(error => console.error('Unable to get items.', error));

}

var startInterval;
function startrun() {
    status = 1;
    document.getElementById('nextShuttle').innerText = document.getElementById('next_starttime').value + ' s';
    document.getElementById('totalTime').innerText = document.getElementById('curr_commulativetime').value + ' s';
    document.getElementById('totalDistance').innerText = document.getElementById('curr_accshuttledistance').value + ' m';

    document.getElementById("curr_track").style.display = "block";
    document.getElementById("btnstartrun").disabled = true;
    document.getElementById("btnstoprun1").disabled = false;
    document.getElementById("btnwarn1").disabled = false;
    document.getElementById("btnstoprun2").disabled = false;
    document.getElementById("btnwarn2").disabled = false;
    document.getElementById("btnstoprun3").disabled = false;
    document.getElementById("btnwarn3").disabled = false;
    startInterval = setInterval(function () {
        var accshuttledist = document.getElementById('curr_accshuttledistance').value;
        // setTimeout(() => { clearInterval(inter); }, millisecond);
        getNextShuttle(accshuttledist)
    }, loadsecond);

    move(loadsecond);
}


var i = 0;
var moveInterval;
function move(msec) {
    if (i == 0) {
        i = 1;
        var intervel = msec / 100;
        var elem = document.getElementById("myBar");
        var width = 1;
        moveInterval = setInterval(frame, intervel);
        function frame() {
            if (width >= 100) {
                clearInterval(moveInterval);
                elem.style.width = "0%";
                i = 0;
            } else {
                width++;
                elem.style.width = width + "%";
            }
        }
    }
}

var stop = 0;
function stoprun(id, name) {
    stop++;
    const accshuttledist = document.getElementById('curr_accshuttledistance').value;
    const item = {
        Id: parseInt(id, 10),
        AthleteName: name,
        AccumulatedShuttleDistance: parseInt(accshuttledist, 10),
        IsWarned: null
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(
            response => {

                if (stop == 3) {
                    clearInterval(startInterval);
                    clearInterval(moveInterval);
                    document.getElementById("myBar").style.width = "0%";
                    i = 0;
                    stop = 0;
                }

                getAthletesResult(id);
            })
        .then(() => {
            document.getElementById("btnstoprun" + id).disabled = true;
            document.getElementById("btnwarn" + id).disabled = true;
            document.getElementById("btnstoprun" + id).value = "Stoped";
        })
        .catch(error => console.error('Unable to add item.', error));


}

function warn(id, name) {

    document.getElementById("btnwarn" + id).disabled = true;
    document.getElementById("btnwarn" + id).value = "Warned";


    //const accshuttledist = document.getElementById('curr_accshuttledistance').value;

    //const item = {
    //    Id: parseInt(id, 10),
    //    AthleteName: name,
    //    AccumulatedShuttleDistance: parseInt(accshuttledist, 10),
    //    IsWarned: true
    //};

    //fetch(uri, {
    //    method: 'POST',
    //    headers: {
    //        'Accept': 'application/json',
    //        'Content-Type': 'application/json'
    //    },
    //    body: JSON.stringify(item)
    //})
    //    .then(() => {
    //        document.getElementById("btnwarn" + id).disabled = true;
    //        document.getElementById("btnwarn" + id).value = "Warned";

    //    })
    //    .catch(error => console.error('Unable to add item.', error));
}


function _displayShuttleNo(id, data) {
    document.getElementById('shuttleno' + id).innerText = `${data.speedLevel} - ${data.shuttleNo}`;
}

function _displayItems(data) {


    var indx = 0;
    data.forEach(item => {

        if (indx == 0) {
            document.getElementById('curr_accshuttledistance').value = item.accumulatedShuttleDistance
            document.getElementById('curr_speedlevel').value = item.speedLevel
            document.getElementById('curr_shuttlenumber').value = item.shuttleNo
            document.getElementById('curr_speed').value = item.speed
            document.getElementById('curr_leveltime').value = item.levelTime
            document.getElementById('curr_commulativetime').value = item.commulativeTime
            document.getElementById('curr_starttime').value = item.startTime

            document.getElementById('level').innerText = item.speedLevel;
            document.getElementById('shuttle').innerText = item.shuttleNo;
            document.getElementById('kmperhr').innerText = item.speed + ' km/h';

            if (status == 1) {
                document.getElementById('totalTime').innerText = `${item.commulativeTime} m`;
                document.getElementById('totalDistance').innerText = `${item.accumulatedShuttleDistance} m`;
            } else {
                document.getElementById('totalTime').innerText = `0:00 m`;
                document.getElementById('totalDistance').innerText = `0 m`;
            }
        }

        if (indx == 1) {
            document.getElementById('next_accshuttledistance').value = item.accumulatedShuttleDistance
            document.getElementById('next_speedlevel').value = item.speedLevel
            document.getElementById('next_shuttlenumber').value = item.shuttleNo
            document.getElementById('next_speed').value = item.speed
            document.getElementById('next_leveltime').value = item.levelTime
            document.getElementById('next_commulativetime').value = item.commulativeTime
            document.getElementById('next_starttime').value = item.startTime

            if (status == 1) {
                document.getElementById('nextShuttle').innerText = `${item.startTime} s`;
            } else {
                document.getElementById('nextShuttle').innerText = `0:00 s`;
            }
        }

        indx = indx + 1;
    });


    var time = document.getElementById('curr_commulativetime').value;
    var timeParts = time.split(":");
    var millisecond = ((timeParts[0] * (60000)) + (timeParts[1] * 1000));


    var starttime = document.getElementById('curr_starttime').value;
    var timeParts2 = starttime.split(":");
    var millisecond2 = ((timeParts2[0] * (60000)) + (timeParts2[1] * 1000));

    loadsecond = millisecond - millisecond2;
    document.getElementById('loadsecond').value = loadsecond;
}