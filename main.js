// function used to sort the rows of the table
// f: either -1 or 1, determines the order the columns will be sorted
// n: which column to sort on
// colId: the id of the column being sorted on
function sortTable(f,n, colId){
	var rows = $('#Parcels tbody  tr').get();
	if (colId == "ADDRESS") {
		rows.sort(function(a,b) {
			var A = getAddressVal(a);
			var B = getAddressVal(b);
			var nameCompare = compare(A[0], B[0]);
			// street names are equal, sort on street number
			if (nameCompare == 0) {
				return compare(A[1], B[1]);
			}
			else {
				return nameCompare;
			}

		});
	}
	else if (colId == "OWNER") {
		rows.sort(function(a,b) {
			var A = getOwnerVal(a);
			var B = getOwnerVal(b);
			return compare(A,B);
		});
	}
	else if (colId == "SALE_DATE") {
		rows.sort(function(a,b) {
			var A = getDateVal(a);
			var B = getDateVal(b);
			return compare(A,B);
		});
	}
	else {
		rows.sort(function(a, b) {
			var A = getVal(a);
			var B = getVal(b);
			return compare(A, B);
		});
	}

	// helper function used to sort rows of the table 
	function compare(A, B) {
		if(A < B) {
			return -1*f;
		}
		if(A > B) {
			return 1*f;
		}
		return 0;
	}

	// takes a row (elm) and returns the specified value of column n in that row
	function getVal(elm){
		var v = $(elm).children('td').eq(n).text().toUpperCase();
		// treat money amounts as numbers
		v = v.replace(/[$,]/g, '');
		if($.isNumeric(v)){
			v = parseInt(v,10);
		}
		return v;
	}	

	// modified version of getVal used to grab the address. Splits it into street name and number
	function getAddressVal(elm) {
		var address = $(elm).children('td').eq(n).text().trim().toUpperCase();
		// can add additional delimiters here if additional data calls for it
		var regex = /\s*-\s*|\s+/;
		var splitAddress = address.split(regex);
		var streetName = splitAddress.slice(1).join(' ');
		var streetNumber = parseInt(splitAddress[0], 10);
		return [streetName, streetNumber];
	}

	// modified version of getVal for the owner column. 
	// sorts the column by first name
	// values in column are either in format "lastName, firstName"
	// or "companyName". Treat companyName as firstName
	function getOwnerVal(elm) {
		var owner = $(elm).children('td').eq(n).text().trim().toUpperCase();
		var splitName = owner.split(',');
		// if > 1, then looking at a persons name, return the their first name
		if (splitName.length > 1) {
			return splitName.slice(1).join(' ').trim();
		}
		// company name, just return the string
		else {
			return splitName[0];
		}
	}

	// modified version of getVal for the date column.
	// converts date string into a date
	function getDateVal(elm) {
		var date = $(elm).children('td').eq(n).text().trim().toUpperCase();
		var parts = date.split('/');
		return new Date(parts[2], parts[0] - 1, parts[1]).getTime();
	}

	// append sorted rows into the HTML file
	$.each(rows, function(index, row) {
		$('#Parcels').children('tbody').append(row);
	});
}
$(function() {
    var headers = document.querySelectorAll("#Parcels th");
    var headerMap = {};
	// map each column id to either -1 or 1. Used to determine whether
	// to sort by ascending or descending
    headers.forEach(function(header) {
        headerMap[header.id] = 1;
    });
	// if a column header is clicked, get the column id, then sort by opposite order as before
    $(".clickable").on("click", function(){    
        var colId = $(this).attr("id");
        headerMap[colId] *= -1;
		// get column index
        var n = $(this).prevAll().length;
        sortTable(headerMap[colId],n, colId);
    });
    
});