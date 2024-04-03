document.addEventListener('DOMContentLoaded', function () {
    select()
    searchQuery()
})

function select() {
    try {
        console.log("inne i select")
        let select = document.querySelector('.select')
        let selected = select.querySelector('.selected')
        let selectOptions = select.querySelector('.select-options')

        selected.addEventListener('click', function () {
            selectOptions.style.display = (selectOptions.style.display === 'block') ? 'none' : 'block'
        })

        let options = selectOptions.querySelectorAll('.option')
        options.forEach(function (option) {
            option.addEventListener('click', function () {
                selected.innerText = this.textContent
                selectOptions.style.display = 'none'
                let category = this.getAttribute('data-value')
                selected.setAttribute('data-value', category)
                updateCoursesByFilters()
            })
        })
    }
    catch { }
}

function searchQuery() {

    try {
        document.querySelector('#searchQuery').addEventListener('keyup', function () {
                updateCoursesByFilters()
        })
    }
    catch { }
}

function updateCoursesByFilters() {
    const category = document.querySelector('.select .selected').getAttribute('data-value') || 'all'
    const searchQuery = document.querySelector('#searchQuery').value

    const url = `/courses?key=e7b38f97-46f2-4e42-8cf2-9e5b6b1b433b&category=${encodeURIComponent(category)}&searchQuery=${encodeURIComponent(searchQuery)}`
    fetch(url)
        .then(res => res.text())
        .then(data => {

            const parser = new DOMParser()
            const dom = parser.parseFromString(data, 'text/html')
            const content = document.querySelector('.content')
            const newDomContent = dom.querySelector('.content')
            console.log(content)
            content.innerHTML = newDomContent.innerHTML

            const pagination = dom.querySelector('.pagination') ? dom.querySelector('.pagination').innerHTML : ''
            document.querySelector('.pagination').innerHTML = pagination
        })
}