updateLoop();


async function updateLoop() {
	while (true) {
		loadXMLDoc("scores.xml", 10);
		await sleep(60);
	}
}
 
function sleep(seconds) {
	var ms = seconds * 1000;
	return new Promise(resolve => setTimeout(resolve, ms));
}

function loadXMLDoc(filepath, leaderboardSize) {
    console.log("Attempting to load XML");
    
    var xmlhttp;
    if (window.XMLHttpRequest) {
        xmlhttp = new XMLHttpRequest();
    } else {
        // code for older browsers
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    
    xmlhttp.onreadystatechange = function() {
        if (this.readyState == 4 && this.status == 200) {
            renderScores(this, "tblScores");
        }
    };
	
    xmlhttp.open("GET", filepath, true);
    xmlhttp.send();
}

function renderScores(xml, tableId) {
    var i;
    var xmlDoc = xml.responseXML;
    
    var table="<tr><th class='rankHeader'>Rank</th>" +
        "<th class='nameHeader'>Name</th>" +
        "<th class='scoreHeader'>Score</th></tr>";
    var x = xmlDoc.getElementsByTagName("score");
    for (i = 0; i < 10; i++) {
		var rank = i + 1;
		var name = " ";
		var score = " ";
		
		if (x.length > i) {
			//console.log("x : " + x.length);
			//console.log("i : " + i);
			name = x[i].getElementsByTagName("name")[0].childNodes[0].nodeValue;
			score = x[i].getElementsByTagName("value")[0].childNodes[0].nodeValue;
		}
        
        console.log("--Adding record for [" + name + "] with the score: " + score);
        
        var row = "";
        row += "<tr>";
        
        // Rank
        row += "<td class='rankCell'>";
        row += rank;
        row += "</td>"
        
        // Name
        row += "<td class='nameCell'>";
        row += name;
        row += "</td>"
        
        // Score
        row += "<td class='scoreCell'>";
        row += score;
        row += "</td>";
        
        row += "</tr>";
        table += row;
    }
    document.getElementById(tableId).innerHTML = table;
}