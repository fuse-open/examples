$(document).ready(function() {

    // Add language-none class to code blocks with no language specified.
    $("pre:not([class]) > code:not([class])").parent().addClass("language-none");

    // language-none blocks that have content starting with "<" are likely UX blocks.
    $("pre.language-none > code").each(function() {
        var block = $(this);
        if (block.text().substring(0, 1) === "<") {
            block.parent().removeClass("language-none").addClass("language-ux");
            Prism.highlightElement(block[0]);
        }
    });

});
