import React, { useState } from 'react';
import Dropdown from 'react-bootstrap/Dropdown';

function Category({name, subcategories,setSelectedSubcategoryData}) {
  const handleSubcategoryClick = (subcategoryUrl) => {
    fetch(`https://localhost:44325/api/getSubcategoryInformation?subcategoryUrl=${encodeURIComponent(subcategoryUrl)}`)
      .then((response) => {
        return response.json();
      })
      .then((data) => {
        setSelectedSubcategoryData(data); 
      })
      .catch((error) => {
        console.error('Error:', error);
      });
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
            <Dropdown.Item key={index} value={subcategory.linkToSubCategory} onClick={() => handleSubcategoryClick(subcategory.linkToSubCategory)}>
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
