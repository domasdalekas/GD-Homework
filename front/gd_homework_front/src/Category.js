import React from 'react';
import Dropdown from 'react-bootstrap/Dropdown';
import * as settings from "./settings.js"

function Category({name, subcategories,setSelectedSubcategoryData}) {
  const apiUrl = settings.API_SERVER;
  const handleSubcategoryClick = (subcategoryUrl, subcategoryName, categoryName) => {
    fetch(apiUrl + `getSubcategoryInformation?subcategoryUrl=${encodeURIComponent(subcategoryUrl)}`)
      .then((response) => {
        return response.json();
      })
      .then((data) => {
        setSelectedSubcategoryData(data); 
      })
      .catch((error) => {
        console.error('Error:', apiUrl);
      });
      let currentCategory = document.getElementById("currentCategory");
      currentCategory.textContent = categoryName;
      let subcategoryClickedName = document.getElementById("subcategoryClicked");
      subcategoryClickedName.textContent = subcategoryName;
  };

  return (
    <>
    <div>
      <Dropdown drop="end" className="dropdown-buttons">
        <Dropdown.Toggle className='dropdown-button' id="dropdown-basic">
          {name}
        </Dropdown.Toggle>
        <Dropdown.Menu>
          {subcategories.map((subcategory, index) => (
            <Dropdown.Item key={index} value={subcategory.linkToSubCategory} onClick={() => handleSubcategoryClick(subcategory.linkToSubCategory, subcategory.name, name)}>
              {subcategory.name}
            </Dropdown.Item>
          ))}
        </Dropdown.Menu>
      </Dropdown>
    </div>
    </>
  );
}


export default Category;
