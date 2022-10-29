$(document).ready(function(){
	var storeMaintenaceAuth = {};
	let timestamp = $.getTimestamp();
	storeMaintenaceAuth['EmployeeId'] = $.session.get("employeeId");
	storeMaintenaceAuth['Signature'] = $.calculateSignature(timestamp);
	storeMaintenaceAuth['Timestamp'] = timestamp;

	$.ajax({ 
		type: "POST",
		url: POST_GETSTOREMAINTREQ_URL,
		data: JSON.stringify(storeMaintenaceAuth),
		dataType: "json",
		contentType : "application/json",
		success: function(storeMaintenances){
			if (storeMaintenances.length === 0) {
				$("#storeMaintenances").html('<div class="mt-5 h1 text-secondary text-center">Nessuna manutenzione in sospeso</div>')
			}
			else {
				for (i in  storeMaintenances) {
					$("#storeMaintenances").append('<div class="container list-container bg-body text-head rounded-lg shadow"><div class="text-head my-2">Manutenzione bicicletta in magazzino <br/>Bicicletta n. <span class="font-weight-bold">'+ storeMaintenances[i].Bike.BikeId +'</span></div><form class="storeMaintenanceForm"><input type="hidden" name="TaskId" value="'+ storeMaintenances[i].TaskId +'"/><div class="mt-2"><button type="submit" class="btn btn-block btn-head mt-2">Prendi in carico</button></div></form></div>');
				}
			}
		},
		error: function(data){
			alert("Errore durante il caricamento delle richieste");
		},
	});
});

$(document).on('submit', 'form', function(event){
	var assignStoreMaintenance = {};
	let timestamp = $.getTimestamp();
	assignStoreMaintenance['EmployeeId'] = $.session.get("employeeId");
	assignStoreMaintenance['Signature'] = $.calculateSignature(timestamp);
	assignStoreMaintenance['Timestamp'] = timestamp;
	assignStoreMaintenance['TaskId'] = $(this).children('input[name=TaskId]').val();


	$.ajax({ 
		type: "POST",
		url: POST_ASSIGN_STOREMAINT_URL,
		data: JSON.stringify(assignStoreMaintenance),
		dataType: "json",
		contentType : "application/json",
		success: function(data) {
			window.location.href = "/Scambici/Employee/HomeEmployee";
		},
		error: function(data) {
			alert("Errore durante la presa in carico della manutenzione");
		},
	});
	event.preventDefault();
});
