import React from 'react';
import Table from 'react-bootstrap/Table';

function SubcategoriesList({ selectedSubcategoryData }) {
  return (
    <div className="table-container">
      {selectedSubcategoryData && (
        <Table>
          <thead>
            <tr>
              <th>Klasifikacija</th>
              <th>Vidutinė kaina</th>
              <th>Kiekis</th>
            </tr>
          </thead>
          <tbody>
            {selectedSubcategoryData.map((subcat, index) => (
              <tr key={index}>
                <td>{subcat.classification}</td>
                <td>{subcat.averagePrice} €</td>
                <td>{subcat.count}</td>
              </tr>
            ))}
          </tbody>
        </Table>
      )}
    </div>
  );
}

export default SubcategoriesList;
