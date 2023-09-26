$(document).ready(function () {
    $("#filter").keyup(function () {
        // Retrieve the input field text and reset the count to zero
        var filter = $(this).val().toLowerCase();

        // Loop through the list items
        $("ul.liveSearchBar li").each(function () {
            // Get the text content of the li element
            var listItemText = $(this).text().toLowerCase();
            
            // If the list item contains the search query, show it; otherwise, hide it
            

            if (listItemText.includes(filter)) {
                
                $(this).show();
            } else {
                
                $(this).hide();
            }
        });

        // Update the count
        var count = $("ul.liveSearchBar li:visible").length;
        $("#filter-count").text("Number of Filter = " + count);
    });
});