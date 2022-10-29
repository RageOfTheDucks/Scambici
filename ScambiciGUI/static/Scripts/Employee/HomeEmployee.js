$(document).ready(function(){
	var currentTask = {};
	let timestamp = $.getTimestamp();
	currentTask['EmployeeId'] = $.session.get("employeeId");
	currentTask['Signature'] = $.calculateSignature(timestamp);
	currentTask['Timestamp'] = timestamp;
	currentTask['BikeId'] = $(this).children('input[name=BikeId]').val();

	var name = $.session.get("name");
	var surname = $.session.get("surname");
	$("#fullname").html(name +" "+ surname);

	$.ajax({ 
		type: "POST",
		url: POST_GETCURRENTTASK_URL,
		data: JSON.stringify(currentTask),
		dataType: "json",
		contentType : "application/json",
		success: function(employee){
			if (employee.TaskDiscriminator == "DeliveryProxy") {
				$("#subtitle").html('Al momento ti stai occupando di:');
				$("#task").html('<div class="container list-container bg-body text-head rounded-lg shadow border-lg-head text-left"><div class="text-head mb-2">Consegna per <span class="text-lg-head font-weight-bold">'+ employee.CurrentTask.CreatedBy.Name +' '+ employee.CurrentTask.CreatedBy.Surname +'</span><br/>Data e ora: <span class="font-weight-bold">'+ employee.CurrentTask.DesiredDateTime +'</span><br/>Indirizzo: <span class="font-weight-bold">'+ employee.CurrentTask.CreatedBy.Address +'</span></div><form id="deliveryForm"><div class="mt-3"><input type="text" class="form-control" placeholder="Id bicicletta consegnata" id="BikeId" name="BikeId"/></div><div class="mt-2"><button type="submit" class="btn btn-block btn-head mt-2">Completa</button></div></form></div></div>');
			}
			else if(employee.TaskDiscriminator == "UserMaintenanceProxy") {
				$("#subtitle").html('Al momento ti stai occupando di:');
				$("#task").html('<div class="container list-container bg-body text-head rounded-lg shadow border-lg-head text-left"><div class="text-head my-2">Manutenzione richiesta da <br class="d-sm-none" \><span class="text-lg-head font-weight-bold">'+ employee.CurrentTask.CreatedBy.Name +' '+ employee.CurrentTask.CreatedBy.Surname +'</span><br/><span class="font-weight-bold">Dettagli: </span>'+ employee.CurrentTask.FaultDescription +'</div><form id="userMaintenanceForm"><div class="mt-3"><input type="text" class="form-control" placeholder="ID bicicletta sostitutiva" id="BikeId" name="BikeId"/></div><div class="row"><div class="col-sm-4"><button type="submit" class="btn btn-block btn-outline-head mt-2" id="swap">Sostituisci bici</button></div><div class="col-sm-8"><button type="submit" class="btn btn-block btn-head mt-2" id="terminate">Completa</button></div></div></form></div>');
			}
			else if(employee.TaskDiscriminator == "StorageMaintenanceProxy") {
				$("#subtitle").html('Al momento ti stai occupando di:');
				$("#task").html('<div class="container list-container bg-body text-head rounded-lg shadow border-lg-head text-left"><div class="text-head my-2">Manutenzione bicicletta in magazzino <br/>Bicicletta n. <span class="font-weight-bold">'+ employee.CurrentTask.Bike.BikeId +'</span></div><form id="storeMaintenanceForm"><div class="row"><div class="col-sm-4"><button type="submit" class="btn btn-block btn-outline-head mt-2" id="scrap">Rottama bici</button></div><div class="col-sm-8"><button type="submit" class="btn btn-block btn-head mt-2" id="terminate">Completa</button></div></div></form></div>');
			}
			else {
				$("#subtitle").html('Al momento non ti stai occupando di alcun task.');
			}
		},
		error: function(data){
			alert("Errore durante il caricamento del task corrente");
		},
	});
});

$(document).on('submit', '#deliveryForm', function(event){
	var terminateDelivery = {};
	let timestamp = $.getTimestamp();
	terminateDelivery['EmployeeId'] = $.session.get("employeeId");
	terminateDelivery['Signature'] = $.calculateSignature(timestamp);
	terminateDelivery['Timestamp'] = timestamp;
	terminateDelivery['BikeId'] = $('#BikeId').val();

	$.ajax({ 
		type: "POST",
		url: POST_TERMINATE_DELIVERY_URL,
		data: JSON.stringify(terminateDelivery),
		dataType: "json",
		contentType : "application/json",
		success: function(data) {
			window.location.href = "/Scambici/Employee/HomeEmployee";
		},
		error: function(data) {
			alert("Errore durante la terminazione della consegna");
		},
	});
	event.preventDefault();
});

var target;
$(document).on('click', 'button[type=submit]', function() {
	target = $(this).attr('id');
});

$(document).on('submit', '#userMaintenanceForm', function(event){
	var terminateUserMaintenance = {};
	let timestamp = $.getTimestamp();
	terminateUserMaintenance['EmployeeId'] = $.session.get("employeeId");
	terminateUserMaintenance['Signature'] = $.calculateSignature(timestamp);
	terminateUserMaintenance['Timestamp'] = timestamp;

	if(target == "swap"){
		terminateUserMaintenance['BikeId'] = $('#BikeId').val();
		$.ajax({ 
			type: "POST",
			url: POST_BIKESWAP_URL,
			data: JSON.stringify(terminateUserMaintenance),
			dataType: "json",
			contentType : "application/json",
			success: function(data) {
				$.ajax({ 
					type: "POST",
					url: POST_TERMINATE_USERMAINT_URL,
					data: JSON.stringify(terminateUserMaintenance),
					dataType: "json",
					contentType : "application/json",
					success: function(data) {
						window.location.href = "/Scambici/Employee/HomeEmployee";
					},
					error: function(data) {
						alert("Errore durante la terminazione della manutenzione utente");
					},
				});
			},
			error: function(data) {
				alert("Errore durante la sostituzione della bicicletta");
			},
		});
	}
	if (target == "terminate") {
		$.ajax({ 
			type: "POST",
			url: POST_TERMINATE_USERMAINT_URL,
			data: JSON.stringify(terminateUserMaintenance),
			dataType: "json",
			contentType : "application/json",
			success: function(data) {
				window.location.href = "/Scambici/Employee/HomeEmployee";
			},
			error: function(data) {
				alert("Errore durante la terminazione della manutenzione utente");
			},
		});
	}
	event.preventDefault();
});

$(document).on('submit', '#storeMaintenanceForm', function(event){
	var terminateStoreMaintenance = {};
	let timestamp = $.getTimestamp();
	terminateStoreMaintenance['EmployeeId'] = $.session.get("employeeId");
	terminateStoreMaintenance['Signature'] = $.calculateSignature(timestamp);
	terminateStoreMaintenance['Timestamp'] = timestamp;

	if(target == "scrap"){
		$.ajax({ 
			type: "POST",
			url: POST_BIKESCRAP_URL,
			data: JSON.stringify(terminateStoreMaintenance),
			dataType: "json",
			contentType : "application/json",
			success: function(data) {
				$.ajax({ 
					type: "POST",
					url: POST_TERMINATE_STOREMAINT_URL,
					data: JSON.stringify(terminateStoreMaintenance),
					dataType: "json",
					contentType : "application/json",
					success: function(data) {
						window.location.href = "/Scambici/Employee/HomeEmployee";
					},
					error: function(data) {
						alert("Errore durante la terminazione della manutenzione magazzino");
					},
				});
			},
			error: function(data) {
				alert("Errore durante la rottamazione della bicicletta");
			},
		});
	}
	if(target == "terminate") {
		$.ajax({ 
			type: "POST",
			url: POST_TERMINATE_STOREMAINT_URL,
			data: JSON.stringify(terminateStoreMaintenance),
			dataType: "json",
			contentType : "application/json",
			success: function(data) {
				window.location.href = "/Scambici/Employee/HomeEmployee";
			},
			error: function(data) {
				alert("Errore durante la terminazione della manutenzione magazzino");
			},
		});
	}
	event.preventDefault();
});
