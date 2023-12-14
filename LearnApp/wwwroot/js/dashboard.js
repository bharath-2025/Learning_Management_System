let profile = document.querySelector('.header .flex .profile');
let search = document.querySelector('.header .flex .search-bar');
let sidebar = document.querySelector('.side-bar');
let closeIcon = document.querySelector('.side-bar .close-side-bar');
let body = document.body;

document.querySelector('#user-btn').onclick = () =>{
    console.log("clicked");
    //toggle the profile and close all other active classes
    profile.classList.toggle('active');
    search.classList.remove('active');
}

document.querySelector('#search-btn').onclick = () =>{
    console.log("Search clicked");
     //toggle the search and close all other active classes
    search.classList.toggle('active');
    profile.classList.remove('active');
    sidebar.classList.remove('active');
}

document.querySelector("#menu-btn").onclick = ()=>{
     //toggle the sideBar and close all other active classes
    sidebar.classList.toggle('active');
    body.classList.toggle('active');
    search.classList.remove('active');
    profile.classList.remove('active');
}

closeIcon.onclick = ()=>{
    sidebar.classList.remove('active');
    body.classList.remove('active');
}

window.onscroll = () =>{
    profile.classList.remove('active');
    search.classList.remove('active');
}