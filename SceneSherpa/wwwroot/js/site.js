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

// JavaScript function to store and restore scroll position
function storeAndRestoreScrollPosition() {
    // Store the current scroll position in localStorage
    localStorage.setItem('scrollPosition', window.scrollY);

    // Attach an event listener to the window's load event
    window.addEventListener('load', function () {
        // Retrieve the stored scroll position from localStorage
        var scrollPosition = localStorage.getItem('scrollPosition');

        // If a scroll position was stored, scroll to it
        if (scrollPosition !== null) {
            window.scrollTo(0, parseInt(scrollPosition));
            localStorage.removeItem('scrollPosition'); // Clear the stored position
        }
    });
}